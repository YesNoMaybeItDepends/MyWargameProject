using System;
using Godot;
using System.Collections.Generic;
using Priority_Queue;

public static class Pathfinding
{
    public static List<Hex> GetPath(Hex start, Hex goal)
    {
        return reconstructPath(AStarSearch(start,goal).cameFrom, start, goal);
    }

    // With early exit. It could be used to find an area without the early exit.
    static Dictionary<Hex, Hex> BreadthFirstSearch(Hex start, Hex goal)
    {
        Queue<Hex> frontier = new Queue<Hex>();
        frontier.Enqueue(start);

        Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>();
        cameFrom[start] = null;

        while (frontier.Count != 0)
        {
            Hex current = frontier.Dequeue();

            if (current == goal) // Early Exit
            {
                break;
            }

            foreach (Hex next in current.GetNeighbours())
            {
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        return cameFrom;
    }

    // Dijkstra's Algorithm
    static (Dictionary<Hex,Hex> cameFrom, Dictionary<Hex, int> costSoFar) UniformCostSearch(Hex start, Hex goal)
    {
        SimplePriorityQueue<Hex> frontier = new SimplePriorityQueue<Hex>();
        frontier.Enqueue(start,0);

        Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>();
        cameFrom[start] = null;
        
        Dictionary<Hex, int> costSoFar = new Dictionary<Hex, int>();
        costSoFar[start] = 0;

        while (frontier.Count != 0)
        {
            Hex current = frontier.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach(Hex next in current.GetNeighbours())
            {
                int newCost = costSoFar[current] + next.terrain.MovementCost;
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost;
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        return (cameFrom, costSoFar);
    }

    static Dictionary<Hex, Hex> GreedyBestFirstSearch(Hex start, Hex goal)
    {
        SimplePriorityQueue<Hex> frontier = new SimplePriorityQueue<Hex>();
        frontier.Enqueue(start, 0);

        Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>();
        cameFrom[start] = null;

        while (frontier.Count != 0)
        {
            Hex current = frontier.Dequeue();

            if (current == goal)
            {
                break;

            }

            foreach (Hex next in current.GetNeighbours())
            {
                if (!cameFrom.ContainsKey(next))
                {
                    int priority = goal.axialPos.DistanceTo(next.axialPos);
                    frontier.Enqueue(next,priority);
                    cameFrom[next] = current;
                }
            }
        }

        return cameFrom;
    }

    static (Dictionary<Hex,Hex> cameFrom, Dictionary<Hex, int> costSoFar) AStarSearch(Hex start, Hex goal)
    {
        SimplePriorityQueue<Hex> frontier = new SimplePriorityQueue<Hex>();
        frontier.Enqueue(start, 0);
        
        Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>();
        cameFrom[start] = null;
        
        Dictionary<Hex, int> costSoFar = new Dictionary<Hex, int>();
        costSoFar[start] = 0;

        while (frontier.Count != 0)
        {
            Hex current = frontier.Dequeue();

            if (current == goal)
            {
                break;   
            }

            foreach (Hex next in current.GetNeighbours())
            {
                int newCost = costSoFar[current] + next.terrain.MovementCost;
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + goal.axialPos.DistanceTo(next.axialPos);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        return (cameFrom, costSoFar);
    }

    static List<Hex> reconstructPath(Dictionary<Hex, Hex> cameFrom, Hex start, Hex goal)
    {
        Hex current = goal;
        List<Hex> path = new List<Hex>();

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }

}