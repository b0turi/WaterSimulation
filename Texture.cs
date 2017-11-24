using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterSimulation
{
    public class Texture
    {
        public int texID;
        public int width;
        public int height;
        public List<byte> buffer;

        public Texture(int texID, int width, int height)
        {
            this.texID = texID;
            this.width = width;
            this.height = height;
        }
    }
}
