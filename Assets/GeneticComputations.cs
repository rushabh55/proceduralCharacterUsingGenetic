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
        public List<double> scale = new List<double>();
        public List<double> position = new List<double>();
        public Chromosome(int size)
        {
			try
			{
            for (int i = 0; i < size; i++)
            {
					scale.Add(double.MaxValue);
					position.Add(double.MaxValue);
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
        private bool historyPresent = false;
        private System.Random rand = new System.Random();
        public static Chromosome bestFit = new Chromosome(10);
        private double min = double.MaxValue;
        public GeneticComputations(int noOfGenerations, int problemSize)
        {
			getNewPopulation(problemSize, true);
            for (int i = 0; i < noOfGenerations; i++)
            {
                getNewPopulation(problemSize, false);
                selection();
            }
            Debug.Log("Data Ready");
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
                for (int i = 0; i < 5; i++)
                {
                    Chromosome c = new Chromosome(problemSize);
                    for (int j = 0; j < c.scale.Count; j++)
                    {
                        c.scale[j] = rand.NextDouble();
                    }
                    for (int j = 0; j < c.position.Count; j++)
                    {
                        c.position[j] = rand.NextDouble();
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
			double bodyAverage = c.position[0] + c.scale[0];
			bodyAverage = bodyAverage / 4;

            for (int i = 1; i < c.position.Count; i++)
            {
				average += c.scale[i];
            }

            average = average / c.position.Count - 1;

            if (average < min)
            {
                min = average;
                bestFit = c;
                Debug.Log("Max : " + average);
            }
            return average;
        }

        public void selection()
        {
            var _population = population.ToArray();
            foreach (var child in _population)
                if (fitnessFuction(child) < 1)
                {
                    population.Remove(child);
                }
                else
                    mutation(child);

        }

        public void crossOver()
        {
            if (population != null)
                if (population.FirstOrDefault().scale.Count <= 2)
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
                int pointOfCrossover = rand.Next(0, ch.scale.Count);
                while (pointOfCrossover < 2)
                {
                    pointOfCrossover = rand.Next(0, ch.scale.Count);
                }
                var c1 = ch.scale.Skip(pointOfCrossover).ToArray();
                var c2 = ch2.scale.Take(pointOfCrossover).ToArray();
                Debug.Log("POC " + pointOfCrossover + " size : " + ch.position.Count);
                for (int i = 0; i < ch.position.Count; i++)
                {
                    try
                    {
                        if (i < pointOfCrossover)
                            ch2.scale[i] = c1[i];
                        else
                            ch2.scale[i] = c2[pointOfCrossover - i];
                    }
                    catch (Exception) { }
                }

                var c3 = ch.scale.Take(pointOfCrossover).ToArray();
                var c4 = ch2.scale.Skip(pointOfCrossover).ToArray();
                for (int i = 0; i < ch.position.Count; i++)
                {
                    try
                    {
                        if (i < pointOfCrossover)
                            ch.scale[i] = c4[i];
                        else
                            ch.scale[i] = c3[pointOfCrossover - i];
                    }
                    catch (Exception) { }
                }

                c1 = ch.position.Skip(pointOfCrossover).ToArray();
                c2 = ch2.position.Take(pointOfCrossover).ToArray();
                for (int i = 0; i < ch.position.Count; i++)
                {
                    try
                    {
                        if (i < pointOfCrossover)
                            ch2.position[i] = c1[i];
                        else
                            ch2.position[i] = c2[pointOfCrossover - i];
                    }
                    catch (Exception) { }
                }

                c3 = ch.position.Take(pointOfCrossover).ToArray();
                c4 = ch2.position.Skip(pointOfCrossover).ToArray();
                for (int i = 0; i < ch.position.Count; i++)
                {
                    try
                    {
                        if (i < pointOfCrossover)
                            ch.position[i] = c4[i];
                        else
                            ch.position[i] = c3[pointOfCrossover - i];
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
			if(population.FirstOrDefault().scale.Count <= 2)
				return;
            int pointOfMutation = rand.Next(0, c.position.Count);
            c.position[pointOfMutation] = rand.NextDouble();
            c.scale[pointOfMutation] = rand.NextDouble();
        }

        public static string var_dump()
        {
            if (population != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Distance\n");
                foreach (var w in bestFit.position)
                    sb.Append(w + " ");
                sb.Append("\nScale\n");
                foreach (var w in bestFit.scale)
                    sb.Append(w + " ");
                
                //foreach (var p in population)
                //{
                //    //sb.Append("\nFor the " + p + "child");
                //    sb.Append("\nscale : ");
                //    foreach (var w in p.scale)
                //        sb.Append(" " + w + " ");
                //    sb.Append("\npopulation : ");
                //    foreach (var w in p.position)
                //        sb.Append(" " + w + " ");
                //}

                return sb.ToString();
            }
            else
                return "";
            
        }
    }
}

