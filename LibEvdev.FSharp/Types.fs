namespace LibEvdev.FSharp

open System
open LibEvdev.Native
open LanguagePrimitives

[<AutoOpen>]
module Event =
    [<Struct>]
    type KeyAction =
        | Press
        | Release
        | Repeat

    /// <summary>Converts raw int value to KeyAction.</summary>
    /// <seealso cref="KeyAction"/>
    /// <exception cref="ArgumentOutOfRangeException">Key events must be 0, 1 or 2.</exception>
    let keyActionOfValue value =
        match value with
        | 1 -> Press
        | 0 -> Release
        | 2 -> Repeat
        | _ -> raise (ArgumentOutOfRangeException (nameof value, "Key events must be 0, 1 or 2."))

    let valueOfKeyAction action =
        match action with
        | Press -> 1
        | Release -> 0
        | Repeat -> 2

    /// Represents `libevdev` input event information with more
    /// readable fields and enum members.
    [<Struct>]
    type public InputEvent =
        | SyncEvent of Type: Synchronization * Value: int * Time: DateTime
        | KeyEvent of Key: Key * Action: KeyAction * Time: DateTime
        | RelativeEvent of Axis: Relative * Step: int * Time: DateTime
        | AbsoluteEvent of Abs: Absolute * Value: int * Time: DateTime
        | MiscEvent of Code: Miscellaneous * Value: int * Time: DateTime
        | SwitchEvent of Switched: Switch * NewState: int * Time: DateTime
        | LedEvent of Led: Led * Value: int * Time: DateTime
        | SoundsEvent of Sound: Sounds * Value: int * Time: DateTime
        | RepeatEvent of Repeat: Repeat * Duration: int * Time: DateTime

    let inline private enum16<'TEnum when 'TEnum: enum<uint16>> (value: uint16) =
        EnumOfValue<uint16, 'TEnum> value

    let eventFromRaw (rawEvent: InputEventRaw) =
        let code = rawEvent.Code
        let value = rawEvent.Value
        let time = rawEvent.TimeValue.AsDateTime()
        match rawEvent.Type with
        | EventType.Synchronization -> Some (SyncEvent (enum16<Synchronization> code, value, time))
        | EventType.Key -> Some (KeyEvent (enum16<Key> code, keyActionOfValue value, time))
        | EventType.Relative -> Some (RelativeEvent (enum16<Relative> code, value, time))
        | EventType.Absolute -> Some (AbsoluteEvent (enum16<Absolute> code, value, time))
        | EventType.Miscellaneous -> Some (MiscEvent (enum16<Miscellaneous> code, value, time))
        | EventType.Switch -> Some (SwitchEvent (enum16<Switch> code, value, time))
        | EventType.Led -> Some (LedEvent (enum16<Led> code, value, time))
        | EventType.Sounds -> Some (SoundsEvent (enum16<Sounds> code, value, time))
        | EventType.Repeat -> Some (RepeatEvent (enum16<Repeat> code, value, time))
        | _ -> None

    let toTimeVal (dateTime: DateTime) =
        TimeValue (dateTime.Second, dateTime.Microsecond)

    let rawFromEvent inputEvent =
        match inputEvent with
        | SyncEvent (code, value, time) -> InputEventRaw (toTimeVal time, EventType.Synchronization, uint16 code, value)
        | KeyEvent (code, action, time) -> InputEventRaw (toTimeVal time, EventType.Key, uint16 code, valueOfKeyAction action)
        | RelativeEvent (code, step, time) -> InputEventRaw (toTimeVal time, EventType.Relative, uint16 code, step)
        | AbsoluteEvent (code, value, time) -> InputEventRaw (toTimeVal time, EventType.Absolute, uint16 code, value)
        | MiscEvent (code, value, time) -> InputEventRaw (toTimeVal time, EventType.Miscellaneous, uint16 code, value)
        | SwitchEvent (code, value, time) -> InputEventRaw (toTimeVal time, EventType.Switch, uint16 code, value)
        | LedEvent (code, value, time) -> InputEventRaw (toTimeVal time, EventType.Led, uint16 code, value)
        | SoundsEvent (code, value, time) -> InputEventRaw (toTimeVal time, EventType.Sounds, uint16 code, value)
        | RepeatEvent (code, duration, time) -> InputEventRaw (toTimeVal time, EventType.Repeat, uint16 code, duration)
