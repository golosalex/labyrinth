

using labyrinth.Model;
using labyrinth.ViewModel;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Console.WriteLine(AlgorytmEnum.Prima);
        LabyrinthModel labyrinthModel = new LabyrinthModel();
        labyrinthModel.SomeCellChenged += LabyrinthModel_SomeCellChenged;
        labyrinthModel.CellsChanged += LabyrinthModel_CellsChanged;

        labyrinthModel.LabyrinthData[2, 2].LeftWall = false;
        labyrinthModel.LabyrinthData[3, 2].LeftWall = true;


        labyrinthModel.ChangeSize(20, 20);
        labyrinthModel.LabyrinthData[2, 2].LeftWall = false;
        labyrinthModel.LabyrinthData[3, 2].LeftWall = true;
        labyrinthModel.LabyrinthData[12, 2].LeftWall = false;
        labyrinthModel.LabyrinthData[3, 12].LeftWall = true;

        labyrinthModel.ChangeSize(5, 5);
        //labyrinthModel.LabyrinthData[12, 2].LeftWall = false;


        var VM = new LabyrinthViewModel(labyrinthModel,new labyrinth.Launcher());

        labyrinthModel.ChangeSize(20, 20);
        labyrinthModel.LabyrinthData[2, 2].LeftWall = false;
        labyrinthModel.LabyrinthData[3, 2].LeftWall = true;
        labyrinthModel.LabyrinthData[12, 2].LeftWall = false;
        labyrinthModel.LabyrinthData[3, 12].LeftWall = true;

        
    }

    private static void LabyrinthModel_CellsChanged(object? sender, labyrinth.Common.ReadOnly2DArray<Cell> e)
    {
        Console.WriteLine($"размер поля изменился {e.Rows}, {e.Colomns}");
    }

    private static void LabyrinthModel_SomeCellChenged(object? sender, Cell e)
    {
        Console.WriteLine($"ячейка {e.Row}, {e.Colomn} была изменена");
    }
}