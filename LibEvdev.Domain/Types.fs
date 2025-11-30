namespace LibEvdev.Domain

open System
open LibEvdev.Native

module Events = 

    type KeyAction = 
        | Press
        | Release
        | Repeat

    type InputEvent = 
        | SyncEvent
        | KeyEvent of Code: Key * Action: KeyAction * Time: DateTime
        | RelativeEvent of Code: RelativeAxis * Step: int * Time: DateTime
        | MiscellaneousEvent of Code: Miscellaneous * Value: int * Time: DateTime
        | RepeatEvent of Code: Repeat * Duration: int * Time: DateTime

