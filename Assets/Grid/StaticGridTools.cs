using System;
using Grid;
using UnityEngine;


public static class StaticGridTools
{
    private const float hexSize = 10f; // longueur d'un coté hexagone
    private const double sqrt3 = 1.7320508076d;
    
    /// <summary>
    /// Prend un x et un y de la map (array) et renvoie les coordonnées du centre de l'hexagone (un triplet x, y et z)  ; serveur vers client
    /// </summary>
    public static (float, float) MapIndexToWorldCenterCo(int x, int y)
    {
        double xOffset = sqrt3*hexSize; // Décalage horizontal 
        double zOffset = hexSize*3/2; // Décalage vertical 
        double xPos = x * xOffset;
        double zPos = y * zOffset;

        // Décalage pour aligner les lignes impaires
        if (y % 2 == 1)
        {
            xPos += xOffset/2;
        }

        return ((float)xPos, (float)zPos);
    }
    
    /// <summary>
    /// Prend un triplet x, y et z des coordonnées du centre d'un hexagone et renvoie un couple x et y de coordonnées de la map (array) ; Client vers serveur
    /// </summary>
    public static (int,int) WorldCenterCoToMapIndex(float xPos,float yPos, float zPos)
    {
        double xOffset = sqrt3*hexSize; // Décalage horizontal 
        double zOffset = hexSize*3/2; // Décalage vertical 
        
        int y = (int)(zPos / zOffset);
        if (y % 2 == 1)
        {
            xPos -= (float)xOffset / 2;
        }
        int x = (int)(Math.Round(xPos / xOffset));
        return (x, y);
    }
    
    /// <summary>
    /// Prend un couple x et z des coordonnées du centre d'un hexagone et renvoie un couple x et y de coordonnées de la map (array)
    /// </summary>
    public static (int,int) WorldCenterCoToMapIndex(float xPos, float zPos)
    {
        double xOffset = sqrt3*hexSize; // Décalage horizontal 
        double zOffset = hexSize*3/2; // Décalage vertical 
        
        int y = (int)(zPos / zOffset);
        if (y % 2 == 1)
        {
            xPos -= (float)xOffset / 2;
        }
        int x = (int)(Math.Round(xPos / xOffset));
        return (x, y);
    }
    
    /// <summary>
    /// Prend un triplet x, y et z des coordonnées quelconques et renvoie un triplet x, y et z de coordonées du centre de l'hexagone le plus proche
    /// </summary>
    public static (float,float, float) WorldCoToWorldCenterCo(float xPos,float yPos, float zPos)
    {
        double xOffset = sqrt3*hexSize; // Décalage horizontal 
        double zOffset = hexSize*3/2; // Décalage vertical 
        
        float zPosres = (float)(Math.Round(zPos / zOffset) * zOffset);
        
        float xPosres = (float)(Math.Round(xPos / xOffset) * xOffset);
        if ((int)(zPosres/zOffset) % 2 == 1)
        {
            if (Math.Abs(xPosres - xOffset / 2 - xPos) < Math.Abs(xPosres + xOffset / 2 - xPos))
            {
                xPosres -= (float)xOffset / 2;
            }
            else
            {
                xPosres += (float)xOffset / 2;
            }
        }
        return (xPosres, 0, zPosres);
    }

    /// <summary>
    /// Prend un triplet x, y et z des coordonnées quelconques et renvoie un couple x et y de coordonnées de la map (array)
    /// </summary>
    public static (int, int) WorldCoToMapIndex(float xPos, float yPos, float zPos)
    {
        (float centerxPos, float centeryPos, float centerzPos) = WorldCoToWorldCenterCo(xPos, yPos, zPos);
        return WorldCenterCoToMapIndex(centerxPos, centeryPos, centerzPos);
    }

}
