using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
  [Test]
  public void Foo()
  {
    var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
    var app = new GildedRose(items);
    app.UpdateQuality();
    Assert.That(items[0].Name, Is.EqualTo("fixme"));
  }

  // I will Start by writing tests for the existing behaviors before introducing "Conjured" items.
  // This ensures that when refactor, i do not alter the intended functionality.


  [Test]
  [TestCase(10, 0, 0, Category = "Quality is never negative")]
  [TestCase(0, 0, 0, Category = "Quality is never negative")]
  [TestCase(-10, 0, 0, Category = "Quality is never negative")]

  [TestCase(50, 50, 49, Category = "Quality is never above 50")]

  [TestCase(10, 10, 9, Category = "Quality decreases by 1 when SellIn has not passed")]
  [TestCase(1, 10, 9, Category = "Quality decreases by 1 when SellIn has not passed")]

  [TestCase(0, 10, 8, Category = "Quality decreases by 2 when sell by date has passed")]
  [TestCase(-10, 10, 8, Category = "Quality decreases by 2 when sell by date has passed")]

  public void NormalItemTests(int sellIn, int quality, int expectedQuality)
  {
    var expectedSellIn = sellIn - 1; // Decrease SellIn
    var item = new Item { Name = "Normal", SellIn = sellIn, Quality = quality };
    var app = new GildedRose(new List<Item> { item });

    app.UpdateQuality();

    Assert.That(item.SellIn, Is.EqualTo(expectedSellIn));
    Assert.That(item.Quality, Is.EqualTo(expectedQuality));
  }

  [Test]
  [TestCase(0, 0, 1, Description = "Quality increases by 1 when SellIn is 0")]
  [TestCase(10, 50, 50, Description = "Quality is capped at 50")]
  [TestCase(10, 49, 50, Description = "Quality increases by 1 within 50 limit")]
  [TestCase(2, 0, 1, Description = "Quality increases by 1 as it ages")]
  [TestCase(-1, 0, 2, Description = "Quality increases by 2 after SellIn passes")]
  [TestCase(-1, 49, 50, Description = "Quality increases to 50, capped, with SellIn passed")]
  [TestCase(-5, 48, 50, Description = "Quality increases by 2 after sell-by date, up to 50")]
  [TestCase(-5, 50, 50, Description = "Quality stays at 50 even after SellIn passes")]
  public void AgedBrieUpdateQualityTests(int sellIn, int initialQuality, int expectedQualityAfterUpdate)
  {
    var item = new Item { Name = "Aged Brie", SellIn = sellIn, Quality = initialQuality };
    var app = new GildedRose(new List<Item> { item });
    app.UpdateQuality();

    Assert.That(item.SellIn, Is.EqualTo(sellIn - 1), "SellIn does not decrease as expected.");
    Assert.That(item.Quality, Is.EqualTo(expectedQualityAfterUpdate), "Quality update logic failed.");
  }

}