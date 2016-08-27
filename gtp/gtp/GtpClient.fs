module GtpClient

open System.Net.Sockets
open System.Text
open System.Threading

type GtpClient(host, port) =
    let client = new TcpClient(host, port)
    let stream = client.GetStream()
    let buffer = Array.create 1024 0uy
    let read () =
        let count = stream.Read(buffer, 0, buffer.Length)
        let str = Encoding.ASCII.GetString(buffer, 0, count)
        str
    let write (cmd: string) =
        printfn "\nCOMMAND: %s" cmd
        let count = Encoding.ASCII.GetBytes(cmd, 0, cmd.Length, buffer, 0)
        stream.Write(buffer, 0, count)
        stream.Flush()
        Thread.Sleep 100 // TODO: remove
    let coord x y = sprintf "%c%i" "ABCDEFGHJKLMNOPQRST".[x] (y + 1)
    let readSuccess () =
        let result = read ()
        printfn "RESULT: %s" result
        //if result.[0] <> '=' then sprintf "GTP error: %s" result |> failwith
    let simpleCommand cmd =
        write cmd
        readSuccess ()

    member this.BoardSize(size) = sprintf "boardsize %i\n" size |> simpleCommand
    member this.Komi(komi) = sprintf "komi %f\n" komi |> simpleCommand
    member this.Level(level) = sprintf "level %i\n" level |> simpleCommand
    member this.ClearBoard() = simpleCommand "clear_board\n"
    member this.Play(color, x, y) = sprintf "play %c %s\n" color (coord x y) |> simpleCommand
    member this.Board() =
        // Thread.Sleep 100
        // read () |> printfn "EXTRA: %s"
        write "showboard\n"
        Thread.Sleep 100
        read () |> printfn "BOARD: %s"
    member this.Move(color) =
        sprintf "genmove %c\n" color |> simpleCommand
    member this.Quit() =
        simpleCommand "quit\n"
        client.Close()