namespace evtest

open evtest.Output

module Program =
    [<EntryPoint>]
    let main args =
        Console.markupLine (dim (italic "There's nothing here yet..."))
        0
