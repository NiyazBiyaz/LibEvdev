namespace LibEvdev.FSharp.UInput

open System
open LibEvdev.FSharp
open LibEvdev.Devices
open LibEvdev.Native

[<AutoOpen>]
module Cursor =
    let private buttonAction (dev: IWriteOnlyDevice, button, action) =
        dev.WriteFrame (rawFromEvent (KeyEvent (
            match button with
            | Key.BTN_LEFT | Key.BTN_RIGHT | Key.BTN_MIDDLE | Key.BTN_SIDE | Key.BTN_EXTRA -> button
            | _ -> raise (ArgumentException "Unexpected key value. Use code for mouse buttons.")
            ,
            action,
            DateTime.MinValue // Kernel should ignore this value anyway.
        )))

    let press (dev: IWriteOnlyDevice, button) = buttonAction (dev, button, Press)

    let release (dev: IWriteOnlyDevice, button) = buttonAction (dev, button, Release)

    let move (dev: IWriteOnlyDevice, horizontal, vertical, wheel) =
        if horizontal <> 0 || vertical <> 0 || wheel <> 0 then

            // Kernel should ignore this value anyway
            let time = DateTime.MinValue
            if horizontal <> 0 then
                dev.Write (rawFromEvent (RelativeEvent (Relative.REL_X, horizontal, time)))
            if vertical <> 0 then
                dev.Write (rawFromEvent (RelativeEvent (Relative.REL_Y, vertical, time)))
            if wheel <> 0 then
                dev.Write (rawFromEvent (RelativeEvent (Relative.REL_WHEEL, wheel, time)))

            dev.Flush()

    let moveX (dev, step) = move (dev, step, 0, 0)
    let moveY (dev, step) = move (dev, 0, step, 0)
    let moveWheel (dev, step) = move (dev, 0, 0, step)

[<AutoOpen>]
module Keyboard =
    let mutable private pressed = set<Key> []

    let press (dev: IWriteOnlyDevice, key) =
        dev.WriteFrame (rawFromEvent (KeyEvent (key, Press, DateTime.MinValue))) // Kernel should ignore time anyway
        pressed <- pressed.Add key

    let release (dev: IWriteOnlyDevice, key) =
        dev.WriteFrame (rawFromEvent (KeyEvent (key, Release, DateTime.MinValue))) // Kernel should ignore time anyway
        pressed <- pressed.Remove key

    let getPressed() = pressed

    let isPressed key = pressed.Contains key

    let getRepeat (dev: IWriteOnlyDevice) =
        let description = DeviceDescription dev
        if description.RepeatInfo.HasValue then
            Some description.RepeatInfo.Value
        else
            None
