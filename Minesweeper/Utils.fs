﻿namespace Minesweeper

open System
open System.Drawing
open MonoTouch.UIKit
open MonoTouch.Foundation

module utils = 
    let Width = 8 
    let Height = 8 
    let NumberOfMines = 12

    type MinesweeperButton(isMine, countSurrounding) = 
        inherit UIButton() with
        member x.SurroundingMines : int = countSurrounding
        member x.IsMine : bool = isMine
        member x.Activated : bool = false
        member x.ShowFlag : bool = false

    let mutable countMines = 0
    let setMinesAndGetNeighbors =  
        //set mines
        let rand = let r = new Random() in r.NextDouble
        let SetIsMine() = 
            if (countMines >= NumberOfMines) then
                false
            elif (rand() > 0.75) then
                countMines <- countMines + 1
                true
            else
                false
        let mines = Array2D.init Width Height (fun i j -> SetIsMine())

        // get neighbors
        let addNeighbors i j = 
            let indices = [|(i-1,j-1);(i-1,j);(i-1,j+1);(i,j-1);(i,j+1);(i+1,j-1);(i+1,j);(i+1,j+1)|]

            let filterOutsideBounds = function 
                                        | x, y when x < 0 || y <0 || x > Width-1 || y > Height-1 -> false
                                        | _,_ -> true

            Array.filter filterOutsideBounds indices
                |> Array.map (fun (x,y) -> mines.[x,y])
                |> Array.map (fun x -> match x with | true -> 1 | false -> 0) 
                |> Array.sum
        
        let countNeighbors = Array2D.init Width Height addNeighbors
        mines, countNeighbors
    
//    let rows = 
//        let rec createRows length value = 
//            if (value = Height) then
//                [||]
//            else 
//                Array.create length value 
//                    |> Array.append (createRows length (value+1))
//        createRows Width 0 
//
//    let columns = 
//        let rec createColumns height value = 
//            if (value = Width) then
//                [||]
//            else 
//                [|0..height-1|]
//                    |> Array.append (createColumns height (value+1))
//        createColumns Height 0 
