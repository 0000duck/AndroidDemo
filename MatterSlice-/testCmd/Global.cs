using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myconn
{
    //GD25Q16芯片有芯片擦除，块擦除及扇区擦除功能，有页编程功能，读功能是可以读出单个字节。
    public static class Global
    {
        public static int GoneKb = 1024;//b
        public static int GfullMemory = 2048;//kb
        public static int Gblock = 64;//kb
        public static int Gsector = 4;//kb
        public static int Gpage = 256;//b
    }
}
