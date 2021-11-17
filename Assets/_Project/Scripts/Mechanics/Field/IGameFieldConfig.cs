namespace Mechanics.Field
{
  //ToDo: is it needed? maybe we should remove it
  interface IGameFieldConfig
  {
    float Height{ get; }
    float Width { get; }

    int Rows { get; }
    int Columns { get; }
  }
}
