namespace HardCoded.DB; 

 public record Data 
 {
   public int Id {get; set;} 
   public string ? Name { get; set; }
 }

 public class DataDB
 {
   private static List<Data> _data = new List<Data>()
   {
     new Data{ Id=1, Name="Data Chunk One" },
     new Data{ Id=2, Name="Data Chunk Two"},
     new Data{ Id=3, Name="Data Chunk Three"} 
   };

   public static List<Data> GetAllData() 
   {
     return _data;
   } 

   public static Data ? GetData(int id) 
   {
     return _data.SingleOrDefault(item => item.Id == id);
   } 

   public static Data CreateData(Data item) 
   {
     _data.Add(item);
     return item;
   }

   public static Data UpdateData(Data update) 
   {
     _data = _data.Select(item =>
     {
       if (item.Id == update.Id)
       {
         item.Name = update.Name;
       }
       return item;
     }).ToList();
     return update;
   }

   public static void RemoveData(int id)
   {
     _data = _data.FindAll(item => item.Id != id).ToList();
   }
 }