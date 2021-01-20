using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_21
{
    class Program
    {
        static void Main(string[] args)
        {
            var foods = File
                  .ReadAllLines("Input.txt")
                  .Where(s => !string.IsNullOrWhiteSpace(s))
                  .ToList();
            var allFoods = new Foods(foods);
            Console.WriteLine("De etenswaren zijn ingelezen");
            Console.WriteLine($"Oplossing1 = {allFoods.Oplossing1}");
            Console.WriteLine("start met deel 2");
            Console.WriteLine($"Oplossing2 = {allFoods.Oplossing2}");
            Console.WriteLine("De opdracht is af");
        }
    }
    public class Foods
    {
        public List<Food> ListOfFoods { get; }
        public List<Food> NewListOfFoods { get; }
        public List<string> AllIngredients { get; }
        public List<string> AllAllergens { get; }
        public List<string> IngredientsWithAllergen { get; }
        public List<string> IngredientsWithoutAllergen { get; }
        public int Oplossing1 { get; }
        public string Oplossing2 { get; }
        public Foods(List<string> foods)
        {
            ListOfFoods = new List<Food>();
            NewListOfFoods = new List<Food>();
            AllIngredients = new List<string>();
            AllAllergens = new List<string>();
            foreach (string food in foods)
            {
                ListOfFoods.Add(new Food(food));
            }
            foreach (Food food in ListOfFoods)
            {
                foreach (string ingredient in food.Ingredients)
                {
                    if (!AllIngredients.Contains(ingredient)) AllIngredients.Add(ingredient);
                }
                foreach (string allergen in food.Allergens)
                {
                    if (!AllAllergens.Contains(allergen)) AllAllergens.Add(allergen);
                }
            }
            IngredientsWithAllergen = new List<string> (ListOfIngredientsWithAllergen());
            IngredientsWithoutAllergen = new List<string> (ListOfIngredientsWithoutAllergen());
            Oplossing1 = OccurencesOfIngredientsWithoutAllergen();
            NewListOfFoods = MakeNewListOfFoods();
            Oplossing2 = CanonicalDangereousIngredients();
        }
        string CanonicalDangereousIngredients()
        {
            var allergenOfIngredient = new SortedDictionary<string, string>();
            while (allergenOfIngredient.Count < AllAllergens.Count)
            {
                foreach (Food food in NewListOfFoods)
                {
                    foreach (var allergen in food.Allergens)
                    {
                        var perAllergen2 = new List<List<string>>(NewListOfFoods.Where(i => i.Allergens.Contains(allergen)).Select(x => x.Ingredients).ToList());
                        var IngredientsPerAllergen2 = new List<string>(perAllergen2
                           .Skip(1)
                           .Aggregate(new HashSet<String>(perAllergen2.First()), (h, e) => { h.IntersectWith(e); return h; })
                           .ToList());
                        if (IngredientsPerAllergen2.Count == 1)
                        {
                            var ingredient = IngredientsPerAllergen2.First();
                            allergenOfIngredient[allergen] = ingredient;
                        }
                    }
                }
                foreach (Food food in NewListOfFoods)
                {
                    foreach (var allergen in allergenOfIngredient)
                    {
                        if (food.Ingredients.Contains(allergen.Value)) food.Ingredients.Remove(allergen.Value);
                        if (food.Allergens.Contains((string)allergen.Key)) food.Allergens.Remove((string)allergen.Key);
                    }
                }
            }
            var iets = allergenOfIngredient;
            var oplossing2 = "";
            foreach (var allergen in allergenOfIngredient) oplossing2 += "," + allergen.Value; 
            return oplossing2.Substring(1);
        }
        List<string> ListOfIngredientsWithAllergen()
        {
            var perAllergen = new List<List<string>>();
            var possibleIngredientsWithAllergen = new List<List<string>>();
            var perAllergenTotal = new List<List<string>>();

            foreach (var allergen in AllAllergens)
            {
                perAllergen = new List<List<string>> (ListOfFoods.Where(i => i.Allergens.Contains(allergen)).Select(x => x.Ingredients).ToList());
                var IngredientsPerAllergen = new List<string>(perAllergen
                    .Skip(1)
                    .Aggregate(new HashSet<String>(perAllergen.First()),(h, e) => { h.IntersectWith(e); return h; })
                    .ToList());
                perAllergenTotal.Add(IngredientsPerAllergen);
            }
            return perAllergenTotal.SelectMany(x => x).Distinct().ToList(); ;
        }
        List <string> ListOfIngredientsWithoutAllergen()
        {
            return new List<string>(AllIngredients.Except(IngredientsWithAllergen).ToList());
        }
        int OccurencesOfIngredientsWithoutAllergen()
        {
            return ListOfFoods.SelectMany(x => x.Ingredients).ToList().Where(p => IngredientsWithoutAllergen.Any(p2 => p2 == p)).Count();
        }
        List<Food> MakeNewListOfFoods()
        {
            var newListOfFoods = new List<Food>();
            foreach( Food food in ListOfFoods)
            {
                var newIngredients = new List<string> (food.Ingredients.Where(p => IngredientsWithoutAllergen.All(p2 => p2 != p)).ToList());
                newListOfFoods.Add(new Food(food, newIngredients));
            }
            return newListOfFoods;
        }
    }
   
    public class Food
    {
        public List<string> Ingredients { get; set; }
        public List<string> Allergens { get; set; }
        public Food(string food)
        {
            Ingredients = new List<string>();
            Allergens = new List<string>();
            var teksten = food.Split(' ');
            bool isIngredient = true;
            for (var i = 0; i<teksten.Count(); i++)
            {
                var tekst = teksten[i];
                if(isIngredient)
                {
                    if (tekst == "(contains") isIngredient = false;
                    else Ingredients.Add(tekst);
                }
                else
                {
                    Allergens.Add(tekst.Substring(0, tekst.Count() - 1));
                } 
            }
        }
        public Food(Food food, List<string> ingredients)
        {
            Ingredients = new List<string>(ingredients);
            Allergens = new List<string>(food.Allergens);
        }
    }
}
