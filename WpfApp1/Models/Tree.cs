using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Represents a tree object within a WPF Canvas, handling its positioning, movement, and collision logic.
/// </summary>
public class Tree
{
    /// <summary>
    /// Gets or sets the UI element representing the tree in the view.
    /// </summary>
    public UIElement Element { get; set; }

    public double Test { get; set; }

    /// <summary>
    /// Gets or sets the current coordinates of the tree on the canvas.
    /// </summary>
    public Point Coords { get; set; }

    /// <summary>
    /// Gets or sets the height of the tree element.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the tree element.
    /// </summary>
    public double Width { get; set; }

    private readonly Random random;
    private readonly double minDistance = 150;

    /// <summary>
    /// Initializes a new instance of the <see cref="Tree"/> class.
    /// </summary>
    /// <param name="element">The UI element to associate with this tree.</param>
    /// <param name="random">A shared random number generator instance.</param>
    /// <param name="trees">The list of existing trees to avoid overlapping during initial placement.</param>
    public Tree(UIElement element, Random random, List<Tree> trees)
    {
        this.random = random;
        Element = element;
        if (element is FrameworkElement frameworkElement)
        {
            frameworkElement.Loaded += (sender, e) =>
            {
                Height = frameworkElement.RenderSize.Height;
                Width = frameworkElement.RenderSize.Width;
            };
        }
        Coords = new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
        Element.Visibility = Visibility.Visible;
        if (Coords.X == 0 && Coords.Y == 0)
        { 
            PositionTreeOutsideRoad(trees);
        }
    }

    /// <summary>
    /// Randomly positions the tree outside the designated road area (left or right side).
    /// </summary>
    /// <param name="trees">The list of existing trees to check for proximity constraints.</param>
    public void PositionTreeOutsideRoad(List<Tree> trees)
    {
        double roadLeft = 410;
        double roadRight = 1530;
        double x;
        if (random.NextDouble() < 0.5)
        { 
            x = random.NextDouble() * roadLeft;
        }
        else
        {
            x = roadRight + random.NextDouble() * (Application.Current.MainWindow.ActualWidth - roadRight);
        }
        double y = -random.NextDouble() * Application.Current.MainWindow.ActualHeight - 400;
        Coords = new Point(x, y);
        while (IsTooCloseToOtherTrees(trees))
        {
            if (random.NextDouble() < 0.5)
            {
                x = random.NextDouble() * roadLeft;
            }
            else
            {
                x = roadRight + random.NextDouble() * (Application.Current.MainWindow.ActualWidth - roadRight);
            }
            y = -random.NextDouble() * Application.Current.MainWindow.ActualHeight - 400;
            Coords = new Point(x, y);
        }
    }

    /// <summary>
    /// Updates the tree's position based on the game speed and handles recycling when it leaves the screen.
    /// </summary>
    /// <param name="speed">The vertical movement speed.</param>
    /// <param name="trees">The list of all trees for collision and proximity checks.</param>
    public void Move(double speed, List<Tree> trees)
    {
        var canvas = Application.Current.MainWindow.FindName("GameCanvas") as Canvas;
        Coords = new Point(Coords.X, Coords.Y + speed);
        if (Coords.Y > canvas.ActualHeight)
        {
            PositionTreeOutsideRoad(trees);
        }
        List<UIElement> uiElements = trees.ConvertAll(tree => tree.Element);
        if (CheckCollisionWithOtherObjects(uiElements))
        {
            PositionTreeOutsideRoad(trees);
        }
        if (IsTooCloseToOtherTrees(trees))
        {
            PositionTreeOutsideRoad(trees);
        }
        MoveElement();
    }

    /// <summary>
    /// Checks if the tree's current position overlaps with any other UI elements.
    /// </summary>
    /// <param name="allObjects">A list of UI elements to check against.</param>
    /// <returns>True if a collision is detected; otherwise, false.</returns>
    private bool CheckCollisionWithOtherObjects(List<UIElement> allObjects)
    {
        foreach (var obj in allObjects)
        {
            if (obj != this.Element && IsCollision(obj))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines if this tree's bounding box intersects with another UI element's bounding box.
    /// </summary>
    /// <param name="otherElement">The element to check for intersection.</param>
    /// <returns>True if the rectangles intersect; otherwise, false.</returns>
    private bool IsCollision(UIElement otherElement)
    {
        var treeRect = new Rect(Canvas.GetLeft(Element), Canvas.GetTop(Element), Width, Height);
        var otherRect = new Rect(Canvas.GetLeft(otherElement), Canvas.GetTop(otherElement), otherElement.RenderSize.Width, otherElement.RenderSize.Height);
        return treeRect.IntersectsWith(otherRect);
    }

    /// <summary>
    /// Synchronizes the UI element's position on the Canvas with the internal Coords property.
    /// </summary>
    public void MoveElement()
    {
        Canvas.SetLeft(Element, Coords.X);
        Canvas.SetTop(Element, Coords.Y);
        Element.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Checks if the tree is within the minimum allowed distance to any other tree in the list.
    /// </summary>
    /// <param name="trees">The list of trees to check against.</param>
    /// <returns>True if the tree is too close to another; otherwise, false.</returns>
    private bool IsTooCloseToOtherTrees(List<Tree> trees)
    {
        foreach (var tree in trees)
        {
            if (tree != this && CalculateDistanceToTree(tree) < minDistance)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Calculates the Euclidean distance between this tree and another tree.
    /// </summary>
    /// <param name="otherTree">The other tree to measure distance to.</param>
    /// <returns>The distance as a double.</returns>
    private double CalculateDistanceToTree(Tree otherTree)
    {
        double dx = Coords.X - otherTree.Coords.X;
        double dy = Coords.Y - otherTree.Coords.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
