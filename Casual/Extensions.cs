using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual {
    public static class Extensions {

        public static void ShuffleArray<T>(this T[] array) {
            if(array != null && array.Length > 1) {
                for (int i = array.Length - 1; i > 0; i--) {
                    int j = UnityEngine.Random.Range(0, i + 1);
                    T temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }
}
