using labyrinth.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace labyrinth.Model
{
    public class LabyrinthGenerator
    {
        static Random RND = new Random();
        public ReadOnly2DArray<Cell> Data { get; set; }
        public LabyrinthModel Model { get; set; }
        public LabyrinthGenerator(LabyrinthModel model)
        {
            Model = model;
            Data = model.LabyrinthData;
        }

        public void GenerateLabyrinth(CancellationToken ct, AlgorytmEnum algorithm)
        {
            Model.IsGeneratigActive = true;
            switch (algorithm)
            {
                case AlgorytmEnum.Prima:
                    PrimaAlgorithmGenerate(ct);
                    break;
                case AlgorytmEnum.CommonDeepSerch:
                    CommonDeepWalkGenerate(ct);
                    break;
                case AlgorytmEnum.ModifyDeepSerch:
                    ModifyDeepWalkGenerate(ct);
                    break;
            }
            //CommonDeepWalkGenerate(ct);
            //ModifyDeepWalkGenerate(ct);
            //PrimaAlgorithmGenerate(ct);
            Model.IsGeneratigActive=false;
        }

        #region Private Algorithm methods
        private void CommonDeepWalkGenerate(CancellationToken ct)
        {
            // проверка что данные чистые и если нет то очищаем
            foreach (var cell in Data)
            {
                if (ct.IsCancellationRequested) return;
                if (cell.LeftWall == false &&
                    cell.RightWall == false &&
                    cell.TopWall == false &&
                    cell.BottomWall == false &&

                    cell.Status == StatusEnum.Isolated)
                {
                    continue;
                }
                cell.LeftWall = true;
                cell.RightWall = true;
                cell.BottomWall = true;
                cell.TopWall = true;
                cell.Status = StatusEnum.Isolated;
            }
            // сщетчик изолированных ячеек
            int isolatedCells = Data.Rows * Data.Colomns;
            // пушим в стек первый узел
            Stack<Cell> stack = new Stack<Cell>();
            stack.Push(Data[0, 0]);
            Data[0, 0].Status = StatusEnum.InStack;
            isolatedCells--;
            while (stack.Count != 0)
            {
                if (ct.IsCancellationRequested) return;
                List<DirectionEnum> PosiibleDirections = GetPossibleDirections(stack.Peek());
                //если двигаться из верхушки стека некуда, то отходим назад на 1 элемент
                if (PosiibleDirections.Count == 0)
                {
                    Cell topOfStack = stack.Pop();
                    topOfStack.Status = StatusEnum.InLabirinth;
                }
                else
                {
                    stack.Push(MoveTo(stack.Peek(), PosiibleDirections[RND.Next(PosiibleDirections.Count)]));
                }

                Thread.Sleep(Model.TimeSpan);
            }
        }
        private void ModifyDeepWalkGenerate(CancellationToken ct)
        {
            foreach (var cell in Data)
            {
                if (ct.IsCancellationRequested) return;
                if (cell.LeftWall == false &&
                    cell.RightWall == false &&
                    cell.TopWall == false &&
                    cell.BottomWall == false &&

                    cell.Status == StatusEnum.Isolated)
                {
                    continue;
                }
                cell.LeftWall = true;
                cell.RightWall = true;
                cell.BottomWall = true;
                cell.TopWall = true;
                cell.Status = StatusEnum.Isolated;
            }

            HashSet<Stack<Cell>> setOfStacks = new HashSet<Stack<Cell>>();
            Stack<Cell> firstStack = new Stack<Cell>();
            firstStack.Push(Data[0, 0]);
            Data[0, 0].Status = StatusEnum.InStack;
            setOfStacks.Add(firstStack);

            while (setOfStacks.Count > 0)
            {
                if (ct.IsCancellationRequested) return;
                var activeSteck = setOfStacks.ElementAt(RND.Next(setOfStacks.Count));

                List<DirectionEnum> PosiibleDirections = GetPossibleDirections(activeSteck.Peek());

                if (PosiibleDirections.Count == 0)
                {
                    Cell topOfStack = activeSteck.Pop();
                    topOfStack.Status = StatusEnum.InLabirinth;
                    if (activeSteck.Count == 0)
                    {
                        setOfStacks.Remove(activeSteck);
                    }
                }
                else
                {
                    if (PosiibleDirections.Count > 1)
                    {
                        bool isCreateNewBranch = RND.Next(20) == 1;
                        if (isCreateNewBranch && setOfStacks.Count <= 7)
                        {
                            Stack<Cell> stack = new Stack<Cell>();
                            stack.Push(activeSteck.Peek());//небольшая халтура чтобы использовать MowveTo в том виде как есть пришлось вылить воду из чайника.
                            stack.Push(MoveTo(stack.Peek(), PosiibleDirections[RND.Next(PosiibleDirections.Count - 1)]));
                            activeSteck.Push(MoveTo(activeSteck.Peek(), PosiibleDirections[PosiibleDirections.Count - 1]));
                            Stack<Cell> newStack = new Stack<Cell>(); // танец с бубном, чтобы избавиться от первого элемента в стаке.
                            newStack.Push(stack.Peek());

                            setOfStacks.Add(newStack);
                        }
                        else
                        {
                            activeSteck.Push(MoveTo(activeSteck.Peek(), PosiibleDirections[RND.Next(PosiibleDirections.Count)]));
                        }


                    }
                    else
                    {
                        activeSteck.Push(MoveTo(activeSteck.Peek(), PosiibleDirections[RND.Next(PosiibleDirections.Count)]));
                    }

                }
                if (setOfStacks.Count > 0)
                {
                    Thread.Sleep(Model.TimeSpan / setOfStacks.Count);
                }

            }
        }
        private void PrimaAlgorithmGenerate(CancellationToken ct)
        {

            foreach (var cell in Data)
            {
                if (ct.IsCancellationRequested) return;
                if (cell.LeftWall == false &&
                    cell.RightWall == false &&
                    cell.TopWall == false &&
                    cell.BottomWall == false &&

                    cell.Status == StatusEnum.Isolated)
                {
                    continue;
                }
                cell.LeftWall = true;
                cell.RightWall = true;
                cell.BottomWall = true;
                cell.TopWall = true;
                cell.Status = StatusEnum.Isolated;
            }
            List<Cell> cellsInLabyrinth = new List<Cell>();
            HashSet<Cell> cellsInBorderline = new HashSet<Cell>();
            var startCell = Data[0, 0];
            startCell.Status = StatusEnum.InLabirinth;
            cellsInLabyrinth.Add(Data[0, 0]);
            AddCellsToBorderLine(cellsInBorderline, startCell);
            while(cellsInBorderline.Count > 0)
            {
                if (ct.IsCancellationRequested) return;
                Cell cellToAddToLabyrinth = cellsInBorderline.ElementAt(RND.Next(cellsInBorderline.Count));
                var directions = GetPossibleDirections(cellToAddToLabyrinth, StatusEnum.InLabirinth);
                var directionToHome = directions[RND.Next(directions.Count)];
                Cell HoomeCell = GetCellFromDirection(cellToAddToLabyrinth, directionToHome);
                MoveTo(cellToAddToLabyrinth, directionToHome);
                cellToAddToLabyrinth.Status=StatusEnum.InLabirinth;
                HoomeCell.Status=StatusEnum.InLabirinth;
                cellsInBorderline.Remove(cellToAddToLabyrinth);
                AddCellsToBorderLine(cellsInBorderline,cellToAddToLabyrinth);
                Thread.Sleep(Model.TimeSpan);
            }
        }
        #endregion

        #region Private methods
        private void AddCellsToBorderLine(HashSet<Cell> cellsInBorderline, Cell startCell)
        {
            var posssibleDirections = GetPossibleDirections(startCell);
            foreach (var direction in posssibleDirections)
            {
                Cell newCell = GetCellFromDirection(startCell, direction);
                cellsInBorderline.Add(newCell);
                newCell.Status = StatusEnum.InStack;
            }
        }
        private Cell MoveTo(Cell startCell, DirectionEnum direction)
        {
            Cell nextCell;
            switch (direction)
            {
                case DirectionEnum.Up:
                    startCell.TopWall = false;
                    nextCell = Data[startCell.Row - 1, startCell.Colomn];
                    nextCell.BottomWall = false;
                    nextCell.Status = StatusEnum.InStack;
                    break;
                case DirectionEnum.Down:
                    startCell.BottomWall = false;
                    nextCell = Data[startCell.Row + 1, startCell.Colomn];
                    nextCell.TopWall = false;
                    nextCell.Status = StatusEnum.InStack;
                    break;
                case DirectionEnum.Left:
                    startCell.LeftWall = false;
                    nextCell = Data[startCell.Row, startCell.Colomn - 1];
                    nextCell.RightWall = false;
                    nextCell.Status = StatusEnum.InStack;
                    break;
                case DirectionEnum.Right:
                    startCell.RightWall = false;
                    nextCell = Data[startCell.Row, startCell.Colomn + 1];
                    nextCell.LeftWall = false;
                    nextCell.Status = StatusEnum.InStack;
                    break;
                default: throw new Exception("HZ CHTO ETO");

            }
            return nextCell;
        }
        private Cell GetCellFromDirection(Cell startCell, DirectionEnum direction)
        {
            Cell nextCell;
            switch (direction)
            {
                case DirectionEnum.Up:
                    nextCell = Data[startCell.Row - 1, startCell.Colomn];
                    break;
                case DirectionEnum.Down:
                    nextCell = Data[startCell.Row + 1, startCell.Colomn];
                    break;
                case DirectionEnum.Left:
                    nextCell = Data[startCell.Row, startCell.Colomn - 1];
                    break;
                case DirectionEnum.Right:
                    nextCell = Data[startCell.Row, startCell.Colomn + 1];
                    break;
                default: throw new Exception("HZ CHTO ETO");

            }
            return nextCell;
        }
        private List<DirectionEnum> GetPossibleDirections(Cell cell, StatusEnum status = StatusEnum.Isolated)
        {
            List<DirectionEnum> result = new List<DirectionEnum>();
            ;
            if (cell.Colomn != 0 && Data[cell.Row, cell.Colomn - 1].Status == status) result.Add(DirectionEnum.Left);
            if (cell.Row != 0 && Data[cell.Row - 1, cell.Colomn].Status == status) result.Add(DirectionEnum.Up);

            if (cell.Colomn != Data.Colomns - 1 && Data[cell.Row, cell.Colomn + 1].Status == status) result.Add(DirectionEnum.Right);
            if (cell.Row != Data.Rows - 1 && Data[cell.Row + 1, cell.Colomn].Status == status) result.Add(DirectionEnum.Down);

            return result;
        }

        #endregion


    }
}
