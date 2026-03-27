namespace BehaviouralPatterns;

// Интерфейс элемента (фигуры)
using System.Text;

public interface IShape
{
    void Accept(IShapeVisitor visitor);
}

// Интерфейс посетителя
public interface IShapeVisitor
{
    void Visit(Circle circle);
    void Visit(Rectangle rectangle);
    void Visit(Triangle triangle);
    void Visit(Group group);
}

// Конкретные элементы

// Круг
public class Circle : IShape
{
    public double Radius { get; set; }
    public double X { get; set; }
    public double Y { get; set; }

    public Circle(double radius, double x, double y)
    {
        Radius = radius;
        X = x;
        Y = y;
    }

    public void Accept(IShapeVisitor visitor)
    {
        visitor.Visit(this);
    }
}

// Прямоугольник
public class Rectangle : IShape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double X { get; set; }
    public double Y { get; set; }

    public Rectangle(double width, double height, double x, double y)
    {
        Width = width;
        Height = height;
        X = x;
        Y = y;
    }

    public void Accept(IShapeVisitor visitor)
    {
        visitor.Visit(this);
    }
}

// Треугольник
public class Triangle : IShape
{
    public double SideA { get; set; }
    public double SideB { get; set; }
    public double SideC { get; set; }
    public double X { get; set; }
    public double Y { get; set; }

    public Triangle(double sideA, double sideB, double sideC, double x, double y)
    {
        SideA = sideA;
        SideB = sideB;
        SideC = sideC;
        X = x;
        Y = y;
    }

    public void Accept(IShapeVisitor visitor)
    {
        visitor.Visit(this);
    }
}

// Группа фигур (компоновщик)
public class Group : IShape
{
    private List<IShape> _shapes = new List<IShape>();

    public void Add(IShape shape) => _shapes.Add(shape);

    public void Remove(IShape shape) => _shapes.Remove(shape);

    public IEnumerable<IShape> GetShapes() => _shapes;

    public void Accept(IShapeVisitor visitor)
    {
        visitor.Visit(this);
        foreach (IShape shape in _shapes)
            shape.Accept(visitor);
    }
}

// Конкретные посетители //

// 1. Посетитель для расчета площади
public class AreaCalculator : IShapeVisitor
{
    private double _totalArea = 0;

    public void Visit(Circle circle)
    {
        double area = Math.PI * circle.Radius * circle.Radius;
        _totalArea += area;
        Console.WriteLine($"Круг: площадь = {area:F2}");
    }

    public void Visit(Rectangle rectangle)
    {
        double area = rectangle.Width * rectangle.Height;
        _totalArea += area;
        Console.WriteLine($"Прямоугольник: площадь = {area:F2}");
    }

    public void Visit(Triangle triangle)
    {
        // Формула Герона
        double s = (triangle.SideA + triangle.SideB + triangle.SideC) / 2;
        double area = Math.Sqrt(s * (s - triangle.SideA) * (s - triangle.SideB) * (s - triangle.SideC));
        _totalArea += area;
        Console.WriteLine($"Треугольник: площадь = {area:F2}");
    }

    public void Visit(Group group)
    {
        Console.WriteLine($"--- Группа фигур ---");
    }

    public double GetTotalArea() => _totalArea;
}

// 2. Посетитель для расчета периметра
public class PerimeterCalculator : IShapeVisitor
{
    private double _totalPerimeter = 0;

    public void Visit(Circle circle)
    {
        double perimeter = 2 * Math.PI * circle.Radius;
        _totalPerimeter += perimeter;
        Console.WriteLine($"Круг: периметр = {perimeter:F2}");
    }

    public void Visit(Rectangle rectangle)
    {
        double perimeter = 2 * (rectangle.Width + rectangle.Height);
        _totalPerimeter += perimeter;
        Console.WriteLine($"Прямоугольник: периметр = {perimeter:F2}");
    }

    public void Visit(Triangle triangle)
    {
        double perimeter = triangle.SideA + triangle.SideB + triangle.SideC;
        _totalPerimeter += perimeter;
        Console.WriteLine($"Треугольник: периметр = {perimeter:F2}");
    }

    public void Visit(Group group)
    {
        Console.WriteLine($"--- Группа фигур ---");
    }

    public double GetTotalPerimeter() => _totalPerimeter;
}

// 3. Посетитель для масштабирования фигур
public class ScaleVisitor : IShapeVisitor
{
    private double _factor;

    public ScaleVisitor(double factor)
    {
        _factor = factor;
    }

    public void Visit(Circle circle)
    {
        circle.Radius *= _factor;
        Console.WriteLine($"Круг масштабирован: новый радиус = {circle.Radius:F2}");
    }

    public void Visit(Rectangle rectangle)
    {
        rectangle.Width *= _factor;
        rectangle.Height *= _factor;
        Console.WriteLine($"Прямоугольник масштабирован: новые размеры = {rectangle.Width:F2} x {rectangle.Height:F2}");
    }

    public void Visit(Triangle triangle)
    {
        triangle.SideA *= _factor;
        triangle.SideB *= _factor;
        triangle.SideC *= _factor;
        Console.WriteLine($"Треугольник масштабирован: новые стороны = {triangle.SideA:F2}, {triangle.SideB:F2}, {triangle.SideC:F2}");
    }

    public void Visit(Group group)
    {
        Console.WriteLine($"--- Группа фигур масштабируется ---");
    }
}

// 4. Посетитель для экспорта в XML
public class XmlExportVisitor : IShapeVisitor
{
    private StringBuilder _xml = new();

    public void Visit(Circle circle)
    {
        _xml.AppendLine($"  <circle radius='{circle.Radius}' x='{circle.X}' y='{circle.Y}' />");
    }

    public void Visit(Rectangle rectangle)
    {
        _xml.AppendLine($"  <rectangle width='{rectangle.Width}' height='{rectangle.Height}' x='{rectangle.X}' y='{rectangle.Y}' />");
    }

    public void Visit(Triangle triangle)
    {
        _xml.AppendLine($"  <triangle sideA='{triangle.SideA}' sideB='{triangle.SideB}' sideC='{triangle.SideC}' x='{triangle.X}' y='{triangle.Y}' />");
    }

    public void Visit(Group group)
    {
        _xml.AppendLine($"<group>");
        // Содержимое группы будет добавлено при обходе потомков
        _xml.AppendLine($"</group>");
    }

    public string GetXml()
    {
        return "<shapes>\n" + _xml.ToString() + "</shapes>";
    }
}

// 5. Посетитель для подсчета фигур по типам
public class CountVisitor : IShapeVisitor
{
    private int _circles = 0;
    private int _rectangles = 0;
    private int _triangles = 0;
    private int _groups = 0;

    public void Visit(Circle circle) => _circles++;

    public void Visit(Rectangle rectangle) => _rectangles++;

    public void Visit(Triangle triangle) => _triangles++;

    public void Visit(Group group) => _groups++;

    public void DisplayCounts()
    {
        Console.WriteLine($"Круги: {_circles}");
        Console.WriteLine($"Прямоугольники: {_rectangles}");
        Console.WriteLine($"Треугольники: {_triangles}");
        Console.WriteLine($"Группы: {_groups}");
        Console.WriteLine($"Всего фигур: {_circles + _rectangles + _triangles}");
    }
}

// Дополнительный посетитель для отрисовки
public class DrawVisitor : IShapeVisitor
{
    public void Visit(Circle circle)
    {
        Console.WriteLine($"🎨 Рисуем круг радиусом {circle.Radius:F1} в точке ({circle.X}, {circle.Y})");
    }

    public void Visit(Rectangle rectangle)
    {
        Console.WriteLine($"📐 Рисуем прямоугольник {rectangle.Width:F1}x{rectangle.Height:F1} в точке ({rectangle.X}, {rectangle.Y})");
    }

    public void Visit(Triangle triangle)
    {
        Console.WriteLine($"🔺 Рисуем треугольник со сторонами {triangle.SideA:F1}, {triangle.SideB:F1}, {triangle.SideC:F1} в точке ({triangle.X}, {triangle.Y})");
    }

    public void Visit(Group group)
    {
        Console.WriteLine($"📁 Отрисовка группы фигур:");
    }
}

