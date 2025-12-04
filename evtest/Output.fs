namespace evtest

open Spectre.Console

module Output =
    type Color =
        | String
        | Enum
        | Integer
        | Bool
        | Command
        | Param
        | Attention
        | Warning
        | Error
        | Nice

    let getColor = function
        | String -> "#98C379"
        | Enum -> "#56B6C2"
        | Integer -> "#D19A66"
        | Bool -> "#C678DD"
        | Command -> "#61AFEF"
        | Param -> "#E5C07B"
        | Attention -> "#E06C75"
        | Warning -> "#E5C07B"
        | Error -> "#BE5046"
        | Nice -> "#98C379"

    let inline dye color text = $"[%s{getColor color}]%s{text}[/]"
    let inline dyeColor colorValue text = $"[%s{colorValue}]%s{text}[/]"

    let inline style modifier text = $"[%s{modifier}]%s{text}[/]"
    let bold = style "bold"
    let dim = style "dim"
    let italic = style "italic"
    let underline = style "underline"
    let invert = style "invert"
    let conceal = style "conceal"
    let slowblink = style "slowblink"
    let rapidblink = style "rapidlink"
    let strikethrough = style "strikethrough"
    let selfLink = style "link"
    let inline textLink link text = $"[link=%s{link}]%s{text}[/]"

    module Console =
        let inline markupLine text =
            AnsiConsole.MarkupLine text

        let inline markup text =
            AnsiConsole.Markup text
