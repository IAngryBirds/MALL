using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ILBLI.Core.Tools
{
    public static class CloneCopyExtend  
    {
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TModel DeepClone<TModel>(this TModel model)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, model);
                stream.Seek(0, SeekOrigin.Begin);
                return (TModel)bFormatter.Deserialize(stream);
            }
        }
    }

    
}
