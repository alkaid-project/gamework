using System.Collections.Generic;

namespace core
{
    public class Map
    {
        public int Width = 128;
        public int Height = 128;
        public Cell[,] Cells;
        private List<IMapGen> gens;
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Cell[width,height];
        }

        public void AddGen(IMapGen gen)
        {
            gens.Add(gen);
        }

        public void GenMap()
        {
            gens.Sort((i,j)=> i.GetGenType() - j.GetGenType());
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    foreach (var gen in gens)
                    {
                        gen.GenMap(this,i,j);
                    }
                }
            }
        }
    }
}