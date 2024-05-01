using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomNumGen
{
    private System.Random random = new System.Random();
    private int[] values = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    //private double[] probabilities = { 0.1, 0.2, 0.1, 0.15, 0.05, 0.1, 0.1, 0.1, 0.1 };

    public int GenerateBiasedNumber(double[] probabilities)
    {
        checkProbabilitySum(probabilities);
        double randValue = random.NextDouble();
        double cumulativeProbability = 0;

        for (int i = 0; i < values.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randValue < cumulativeProbability)
            {
                return values[i];
            }
        }

        // Fallback to the last value if something goes wrong
        return values[values.Length - 1];
    }

    private void checkProbabilitySum(double[] probabilities)
    {
        double sum = 0.0;
        foreach(double num in probabilities)
        {
            sum += num;
        }
        if(sum != 1.0)
        {
            Debug.LogError("RandomNumGen.cs --: GenerateBiasedNumver :-- Probabilities Sum does not equal 1 : Sum = " + sum);
        }
    }
}
