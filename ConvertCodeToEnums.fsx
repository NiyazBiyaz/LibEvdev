#r "./LibEvdev/bin/Release/net9.0/LibEvdev.dll"

open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open LibEvdev.Native

module Token = 
    let private takeNameValuePairs (line: string) = 
        let regex = @"^#define[ \t]+([A-Za-z0-9_]+)[ \t]+([^/\n]+?)(?:[ \t]*/.*)?$" // Thank you, DeepSeek

        let clearReferenceValue str = 
            let noBrackets = Regex.Replace (str, @"^\((.*)\)$", @"$1")
            Regex.Replace(noBrackets, @"\s+", "")

        let m = Regex.Match(line, regex)
        if m.Success then
            let name = m.Groups[1].Value
            let value = m.Groups[2].Value
            let clearValue = clearReferenceValue value
            Some (name, clearValue)
        else
            None

    let private toInt str =
        let toIntFromDec str = 
            try
                Some(Convert.ToUInt16 (str, 10))
            with
                | :? FormatException -> None

        let toIntFromHex str =
            try
                Some(Convert.ToUInt16 (str, 16))
            with
                | :? FormatException -> None

        let decimal = toIntFromDec str
        let hexadecimal = toIntFromHex str
        if decimal.IsSome then
            decimal
        elif hexadecimal.IsSome then
            hexadecimal
        else 
            None

    type Constant = 
        | Value of string * uint16
        | Reference of string * string

    let private tokenizeConstant (name, value) =
        let parsed = toInt value

        match parsed with
        | Some v -> Value (name, v)
        | None -> Reference (name, value)

    let tokenize (lines: string seq) = 
        lines
        |> Seq.choose takeNameValuePairs 
        |> Seq.map tokenizeConstant

module Event =
    open Token

    type CodeValue = 
        | Number of uint16
        | Ref of string

        override this.ToString () = 
            match this with
            | Number v -> $"0x{v:x}"
            | Ref v -> 
                if v.Contains '+' then
                    String.Join (" + ", v.Split '+')
                else 
                    v

    type EventCode = 
        { Type: EventType
          Code: CodeValue
          Name: string }

        override this.ToString() =
            $"{this.Name} = {this.Code},"

    let private getCodeType (eventName: string) =
        match Array.toList (eventName.Split '_') with
        | "SYN" :: _ -> Some EventType.Synchronization
        | "KEY" :: _ | "BTN" :: _ -> Some EventType.Key
        | "REL" :: _ -> Some EventType.Relative
        | "ABS" :: _ -> Some EventType.Absolute
        | "MSC" :: _ -> Some EventType.Miscellaneous
        | "SW" :: _ -> Some EventType.Switch
        | "LED" :: _ -> Some EventType.Led
        | "SND" :: _ -> Some EventType.Sounds
        | "REP" :: _ -> Some EventType.Repeat
        | _ -> None

    let createEventCode (constant: Constant) =
        match constant with
        | Value (name, value) ->
            match getCodeType name with
            | Some eventType -> Some { Type = eventType; Code = Number value; Name = name }
            | None -> None
        | Reference (name, value) ->
            match getCodeType name with
            | Some eventType -> Some { Type = eventType; Code = Ref value; Name = name }
            | None -> None

let openBlock = @"{"
let closeBlock = @"}"
let newLine = "\n"

module Type =
    let private createEnumHead (eventType: EventType) = 
        $"public enum %s{eventType.ToString()} : ushort"

    let private indent = @"    "
    let private doubleIndent = indent + indent

    let createEnum (eventType: EventType, eventCodes: Event.EventCode seq) =
        let concatLines (lines: string seq) = String.Join ('\n', lines)

        let head = createEnumHead eventType
        let codeLines = eventCodes
                        |> Seq.map (fun code -> doubleIndent + code.ToString())
                        |> concatLines

        concatLines [|
            indent + head
            indent + openBlock
            codeLines
            indent + closeBlock + newLine
        |]

// Entry

let lines = File.ReadAllLines "./input-event-codes.h"
let tokens = Token.tokenize lines
let eventCodes = Seq.choose Event.createEventCode tokens
let grouped = Seq.groupBy (fun (code: Event.EventCode) -> code.Type) eventCodes
let parsed = Seq.map Type.createEnum grouped

let fileStream = new FileStream ("./LibEvdev/Native/InputEventCodes.cs", FileMode.Create)

// License header
fileStream.Write "// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.\n"B
fileStream.Write "// See the LICENSE file in the repository root for full license text.\n"B

fileStream.Write "\n"B

// Alert about auto-generation
fileStream.Write "// This file was automatically generated using /usr/include/linux/input-event-codes.h.\n"B
fileStream.Write "// Run ConvertCodeToEnums.fsx with input-event-codes.h in the same directory if it differs in your system.\n"B

fileStream.Write "\n"B

fileStream.Write "namespace LibEvdev.Native\n"B
fileStream.Write "{\n"B
for enum in parsed do
    fileStream.Write (Encoding.UTF8.GetBytes enum)
fileStream.Write "}\n"B

fileStream.Flush true
fileStream.Dispose()
