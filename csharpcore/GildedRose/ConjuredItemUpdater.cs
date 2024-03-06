namespace GildedRoseKata;

public class ConjuredItemUpdater : IItemUpdater
{
  public void UpdateQuality(Item item)
  {
    if (item.Quality > 0)
    {
      item.Quality -= 2;
    }
    else
    {
      item.Quality = 0;
    }

    item.SellIn -= 1;
  }
}
