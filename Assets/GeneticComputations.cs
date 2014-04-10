using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;


namespace GA
{    
    public class Chromosome
    {       
        public List<double> mass = new List<double>();
        public List<double> force = new List<double>();
        public Chromosome(int size)
        {
			try
			{
                for (int i = 0; i < size; i++)
                {
					    mass.Add(double.MaxValue);
					    force.Add(double.MaxValue);
                }
			}
			catch(Exception) {}
        }
    }

    interface IGeneticComputations
    {
        void getNewPopulation(int problemSize, bool a);
        double fitnessFuction(Chromosome c);
        void selection();
        void crossOver();
        void mutation(Chromosome c);
    }

    class GeneticComputations : IGeneticComputations
    {
        public static List<Chromosome> population = new List<Chromosome>();
        private System.Random rand = new System.Random();
        public static Chromosome bestFit = new Chromosome(10);
        private double max = double.MinValue;
        public GeneticComputations(int noOfGenerations, int problemSize)
        {
			getNewPopulation(problemSize, true);
            for (int i = 0; i < noOfGenerations; i++)
            {
                getNewPopulation(problemSize, false);
                selection();
            }          
        }

        //private void getData()
        //{
        //    XmlSerializer xmlS = new XmlSerializer(typeof(List<Chromosome>));
        //    try
        //    {
        //        FileStream fs = new FileStream("C:/data.xml", FileMode.Open);
        //        population = (List<Chromosome>)xmlS.Deserialize(fs);
        //        fs.Close();
        //        historyPresent = true;
        //    }
        //    catch (Exception _e) {
        //        population = new List<Chromosome>();
        //        historyPresent = false;
        //    }
        //}

        //private void saveData()
        //{
        //    XmlSerializer xmlS = new XmlSerializer(typeof(List<Chromosome>));
        //    try
        //    {
        //        FileStream fs = new FileStream("C:/data.xml", FileMode.OpenOrCreate);
        //        if(population.Count == 0)
        //            getNewPopulation();
        //        xmlS.Serialize(fs, population);
        //        fs.Flush();
        //        fs.Close();
        //    }
        //    catch (Exception _e)
        //    {
        //        population = new List<Chromosome>();
        //    }
        //}

        public void getNewPopulation(int problemSize, bool flag)
        {
			if (flag)
            {
                for (int i = 0; i < 10; i++)
                {
                    Chromosome c = new Chromosome(problemSize);
                    for (int j = 0; j < c.mass.Count; j++)
                    { 
                        c.mass[j] = rand.NextDouble(); 
                        c.force[j] = rand.NextDouble(); 
                    }
                    population.Add(c);
                }
            }
            else
            {
                crossOver();
            }
        }

        public double fitnessFuction(Chromosome c)
        {
            double average = 0;
            for (int i = 0; i < c.force.Count; i++)
            {
                //We want a good amount of force. 
                average += c.force[i];

                //mass is not good. 
                average -= c.mass[i];
            }
            //averaging over all the values inside chromosome. 
            average = average / c.mass.Count;

            //finding the max
            if (average > max)
            {
                max = average;
                bestFit = c;
                Debug.Log("Max : " + average);
            }

            return average;
        }

        public void selection()
        {
            var _population = population.ToArray();
            foreach (var child in _population)
                if (fitnessFuction(child) < threshold)
                {
                    population.Remove(child);
                }
                else
                    mutation(child);

        }

        double threshold = 0;
        public void crossOver()
        {
            if (population != null)
                if (population.FirstOrDefault().mass.Count <= 2)
                    return;
            Dictionary<Chromosome, Chromosome> crossOverData = new Dictionary<Chromosome, Chromosome>();
            for (int i = 0; i < population.Count; i += 2)
            {
                try
                {
                    crossOverData.Add(population[i], population[i + 1]);
                }
                catch (Exception) { }
            }

            var _newPopulation = new List<Chromosome>();
            foreach (var ch in crossOverData.Keys)
            {
                var ch2 = crossOverData[ch];
                int pointOfCrossover = rand.Next(0, ch.mass.Count);
                while (pointOfCrossover < 2)
                {
                    pointOfCrossover = rand.Next(0, ch.mass.Count);
                }
                var c1 = ch.mass.Skip(pointOfCrossover).ToArray();
                var c2 = ch2.mass.Take(pointOfCrossover).ToArray();
                Debug.Log("POC " + pointOfCrossover + " size : " + ch.force.Count);
                for (int i = 0; i < ch.force.Count; i++)
                {
                    try
                    {
                        if (i < pointOfCrossover)
                            ch2.mass[i] = c1[i];
                        else
                            ch2.mass[i] = c2[pointOfCrossover - i];
                    }
                    catch (Exception) { }
                }

                var c3 = ch.mass.Take(pointOfCrossover).ToArray();
                var c4 = ch2.mass.Skip(pointOfCrossover).ToArray();
                for (int i = 0; i < ch.force.Count; i++)
                {
                    try
                    {
                        if (i < pointOfCrossover)
                            ch.mass[i] = c4[i];
                        else
                            ch.mass[i] = c3[pointOfCrossover - i];
                    }
                    catch (Exception) { }
                }

                c1 = ch.force.Skip(pointOfCrossover).ToArray();
                c2 = ch2.force.Take(pointOfCrossover).ToArray();
                for (int i = 0; i < ch.force.Count; i++)
                {
                    try
                    {
                        if (i < pointOfCrossover)
                            ch2.force[i] = c1[i];
                        else
                            ch2.force[i] = c2[pointOfCrossover - i];
                    }
                    catch (Exception) { }
                }

                c3 = ch.force.Take(pointOfCrossover).ToArray();
                c4 = ch2.force.Skip(pointOfCrossover).ToArray();
                for (int i = 0; i < ch.force.Count; i++)
                {
                    try
                    {
                        if (i < pointOfCrossover)
                            ch.force[i] = c4[i];
                        else
                            ch.force[i] = c3[pointOfCrossover - i];
                    }
                    catch (Exception) { }
                }
                _newPopulation.Add(ch);
                _newPopulation.Add(ch2);
            }
            population = _newPopulation;
        }

        public void mutation(Chromosome c)
        {
			if(population.FirstOrDefault().mass.Count <= 2)
				return;
            int pointOfMutation = rand.Next(0, c.force.Count);
            c.force[pointOfMutation] = rand.NextDouble();
            c.mass[pointOfMutation] = rand.NextDouble();
        }

        public static string var_dump()
        {
            if (population != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Force\n");
                foreach (var w in bestFit.force)
                    sb.Append(w + " ");
                sb.Append("\nmass/Fuel\n");
                foreach (var w in bestFit.mass)
                    sb.Append(w + " ");
                
                //foreach (var p in population)
                //{
                //    //sb.Append("\nFor the " + p + "child");
                //    sb.Append("\nmass : ");
                //    foreach (var w in p.mass)
                //        sb.Append(" " + w + " ");
                //    sb.Append("\npopulation : ");
                //    foreach (var w in p.force)
                //        sb.Append(" " + w + " ");
                //}

                return sb.ToString();
            }
            else
                return "";
            
        }
    }
}

