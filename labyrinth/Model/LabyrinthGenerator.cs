using labyrinth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public void GenerateLabyrinth()
        {
            // проверка что данные чистые и если нет то очищаем
            foreach (var cell in Data)
            {
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
                List<DirectionEnum> PosiibleDirections = GetPossibleDirections(stack);
                //если двигаться из верхушки стека некуда, то отходим назад на 1 элемент
                if (PosiibleDirections.Count == 0)
                {
                    Cell topOfStack = stack.Pop();
                    topOfStack.Status = StatusEnum.InLabirinth;
                }
                else
                {
                    MoveTo(stack, PosiibleDirections[RND.Next(PosiibleDirections.Count)]);
                }

                Thread.Sleep(Model.TimeSpan);
            }


        }

        private void MoveTo(Stack<Cell> stack, DirectionEnum directionEnum)
        {
            Cell TopOfStack = stack.Peek();
            Cell nextCell;
            switch (directionEnum)
            {
                case DirectionEnum.Up:
                    TopOfStack.TopWall = false;
                    nextCell = Data[TopOfStack.Row - 1, TopOfStack.Colomn];
                    nextCell.BottomWall = false;
                    nextCell.Status = StatusEnum.InStack;
                    stack.Push(nextCell);
                    break;
                case DirectionEnum.Down:
                    TopOfStack.BottomWall = false;
                    nextCell = Data[TopOfStack.Row + 1, TopOfStack.Colomn];
                    nextCell.TopWall = false;
                    nextCell.Status = StatusEnum.InStack;
                    stack.Push(nextCell);
                    break;
                case DirectionEnum.Left:
                    TopOfStack.LeftWall = false;
                    nextCell = Data[TopOfStack.Row, TopOfStack.Colomn - 1];
                    nextCell.RightWall = false;
                    nextCell.Status = StatusEnum.InStack;
                    stack.Push(nextCell);
                    break;
                case DirectionEnum.Right:
                    TopOfStack.RightWall = false;
                    nextCell = Data[TopOfStack.Row, TopOfStack.Colomn + 1];
                    nextCell.LeftWall = false;
                    nextCell.Status = StatusEnum.InStack;
                    stack.Push(nextCell);
                    break;
                default: throw new Exception("HZ CHTO ETO");

            }
        }
        //todo  строки и столбцы перепутвл. надо поправить
        private List<DirectionEnum> GetPossibleDirections(Stack<Cell> stack)
        {
            List<DirectionEnum> result = new List<DirectionEnum>();
            Cell TopOfStack = stack.Peek();
            if (TopOfStack.Colomn != 0 && Data[TopOfStack.Row, TopOfStack.Colomn - 1].Status == StatusEnum.Isolated) result.Add(DirectionEnum.Left);
            if (TopOfStack.Row != 0 && Data[TopOfStack.Row - 1, TopOfStack.Colomn].Status == StatusEnum.Isolated) result.Add(DirectionEnum.Up);

            if (TopOfStack.Colomn != Data.Colomns - 1 && Data[TopOfStack.Row, TopOfStack.Colomn + 1].Status == StatusEnum.Isolated) result.Add(DirectionEnum.Right);
            if (TopOfStack.Row != Data.Rows - 1 && Data[TopOfStack.Row + 1, TopOfStack.Colomn].Status == StatusEnum.Isolated) result.Add(DirectionEnum.Down);

            return result;
        }
    }
}
