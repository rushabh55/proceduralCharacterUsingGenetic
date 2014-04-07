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
        public double[] scale = new double[5];
        public double[] position = new double[5];
    }

    interface IGeneticComputations
    {
        void getNewPopulation();
        double fitnessFuction(Chromosome c);
        void selection();
        void crossOver();
        void mutation(Chromosome c);
    }

    class GeneticComputations : IGeneticComputations
    {
        public List<Chromosome> population = new List<Chromosome>();
        private bool historyPresent = false;
        private System.Random rand = new System.Random();
        public static Chromosome bestFit = null;
        private double max = double.MinValue;
        public GeneticComputations()
        {
			try
			{
            	getData();
			}
			catch(System.Exception){}
            getNewPopulation();
            selection();
        }

        private void getData()
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(List<Chromosome>));
            try
            {
                FileStream fs = new FileStream("C:/data.xml", FileMode.Open);
                population = (List<Chromosome>)xmlS.Deserialize(fs);
                fs.Close();
                historyPresent = true;
            }
            catch (Exception _e) {
                population = new List<Chromosome>();
                historyPresent = false;
            }
        }

        private void saveData()
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(List<Chromosome>));
            try
            {
                FileStream fs = new FileStream("C:/data.xml", FileMode.OpenOrCreate);
				if(population.Count == 0)
					getNewPopulation();
                xmlS.Serialize(fs, population);
                fs.Flush();
                fs.Close();
            }
            catch (Exception _e)
            {
                population = new List<Chromosome>();
            }
        }

        public void getNewPopulation()
        {
            if (historyPresent)
            {
                for (int i = 0; i < 10; i++)
                {
                    Chromosome c = new Chromosome();
                    for (int j = 0; j < c.scale.Length; j++)
                    {
                        c.scale[j] = rand.NextDouble();
                    }
                    for (int j = 0; j < c.position.Length; j++)
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
			bodyAverage = bodyAverage / 2;

            for (int i = 1; i < c.position.Length; i++)
            {
				average += bodyAverage + c.position[i] + c.scale[i];
            }
            average = average / c.position.Length - 1;
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
                if (fitnessFuction(child) > 0.05)
                {
                    population.Remove(child);
                }
                else
                    mutation(child);

            saveData();
        }

        public void crossOver()
        {
            Dictionary<Chromosome, Chromosome> crossOverData = new Dictionary<Chromosome, Chromosome>();
            for (int i = 0; i < population.Count; i += 2)
            {
                crossOverData.Add(population[i], population[i + 1]);
            }

            var _newPopulation = new List<Chromosome>();
            foreach (var ch in crossOverData.Keys)
            {
                var ch2 = crossOverData[ch];
                int pointOfCrossover = rand.Next(0, ch.scale.Length);
                var c1 = ch.scale.Skip(pointOfCrossover).ToArray();
                var c2 = ch2.scale.Take(pointOfCrossover).ToArray();
                for (int i = 0; i < 5; i++)
                {
                    if (i < pointOfCrossover)
                        ch2.scale[i] = c1[i];
                    else
                        ch2.scale[i] = c2[pointOfCrossover - i];
                }

                var c3 = ch.scale.Take(pointOfCrossover).ToArray();
                var c4 = ch2.scale.Skip(pointOfCrossover).ToArray();
                for (int i = 0; i < 5; i++)
                {
                    if (i < pointOfCrossover)
                        ch.scale[i] = c4[i];
                    else
                        ch.scale[i] = c3[pointOfCrossover - i];
                }

                pointOfCrossover = rand.Next(0, ch.scale.Length);
                c1 = ch.position.Skip(pointOfCrossover).ToArray();
                c2 = ch2.position.Take(pointOfCrossover).ToArray();
                for (int i = 0; i < 5; i++)
                {
                    if (i < pointOfCrossover)
                        ch2.position[i] = c1[i];
                    else
                        ch2.position[i] = c2[pointOfCrossover - i];
                }

                c3 = ch.position.Take(pointOfCrossover).ToArray();
                c4 = ch2.position.Skip(pointOfCrossover).ToArray();
                for (int i = 0; i < 5; i++)
                {
                    if (i < pointOfCrossover)
                        ch.position[i] = c4[i];
                    else
                        ch.position[i] = c3[pointOfCrossover - i];
                }
                _newPopulation.Add(ch);
                _newPopulation.Add(ch2);
            }
            population = _newPopulation;
        }

        public void mutation(Chromosome c)
        {
            int pointOfMutation = rand.Next(0, c.position.Length);
            c.position[pointOfMutation] = rand.NextDouble();
            c.scale[pointOfMutation] = rand.NextDouble();
        }

        public void var_dump()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var p in population)
            {
                sb.Append("\nFor the " + p + "child");
                sb.Append("\nscale : ");
				foreach(var w in p.scale)
                sb.Append(" " + w + " ");
                sb.Append("\npopulation : ");
				foreach(var w in p.position)
					sb.Append(" " + w + " ");
            }

            Debug.Log(sb.ToString());
        }
    }
}

