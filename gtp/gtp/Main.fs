module Main

open GtpClient

type Stone = Black | White
type Point = int * int

type Board() =
    let mutable board = Map.empty<Point, Stone>
    member this.Add(point, stone) = board <- Map.add point stone board
    member this.Remove(point) = board <- Map.remove point board
    member this.Stones() = Map.toList board

let sync (client: GtpClient) (board: Board) =
    client.ClearBoard()
    board.Stones() |> List.iter (fun ((x, y), s) -> client.Play((match s with Black -> 'B' | White -> 'W'), x, y))

// gnugo.exe --quiet --never-resign --monte-carlo --mode gtp --gtp-listen 1234
let client = new GtpClient("localhost", 1234)
client.BoardSize 9
client.Komi 5.5
client.Level 1

let board = new Board()
board.Add((0, 0), Black)
board.Add((1, 1), White)

sync client board

client.Board()
client.Move('W')
client.Move('B')
client.Board()
client.Quit()

open System
open System.Windows.Forms

open Emgu.CV
open Emgu.CV.UI
open Emgu.CV.Structure
 
[<EntryPoint>]
[<STAThread>]
let main argv = 
    let capture = new Capture()
    let img = capture.QueryFrame()
    let viewer = new ImageViewer(img)
    viewer.ShowDialog()
    Application.Idle.Add(fun _ -> viewer.Image <- capture.QueryFrame())
    //Application.EnableVisualStyles()
    //Application.SetCompatibleTextRenderingDefault false
    use form = new Form()
    Application.Run(form);
    0 