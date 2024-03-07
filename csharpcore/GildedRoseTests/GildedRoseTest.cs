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

  public static IEnumerable<TestCaseData> NormalItemTestCaseSource()
  {
    var names = new[] { "+5 Dexterity Vest", "Elixir of the Mongoose" };

    // Test cases for quality decreases correctly
    foreach (var name in names)
    {
      // Test cases for quality never being negative
      // Before sell-by date
      yield return new TestCaseData(10, 0, 0, name).SetCategory("Quality is never negative");
      // On sell-by date
      yield return new TestCaseData(0, 0, 0, name).SetCategory("Quality is never negative");
      // After sell-by date
      yield return new TestCaseData(-10, 0, 0, name).SetCategory("Quality is never negative");

      // Decrease by 1 when SellIn has not passed
      yield return new TestCaseData(10, 10, 9, name).SetCategory("Quality decreases by 1 when SellIn has not passed");
      yield return new TestCaseData(1, 10, 9, name).SetCategory("Quality decreases by 1 when SellIn has not passed");

      // Decrease by 2 when sell by date has passed
      yield return new TestCaseData(0, 10, 8, name).SetCategory("Quality decreases by 2 when sell by date has passed");
      yield return new TestCaseData(-10, 10, 8, name).SetCategory("Quality decreases by 2 when sell by date has passed");

      // Additional case: Quality is capped at 50 for any item increases which isn't expected for normal items but useful to verify the limit
      yield return new TestCaseData(10, 50, 49, name).SetCategory("Quality decreases within 50 limit");
    }
  }



  [Test, TestCaseSource(nameof(NormalItemTestCaseSource))]
  public void NormalItemTests(int sellIn, int quality, int expectedQuality, string name)
  {
    var expectedSellIn = sellIn - 1; // Decrease SellIn
    var item = new Item { Name = name, SellIn = sellIn, Quality = quality };
    var app = new GildedRose(new List<Item> { item });

    app.UpdateQuality();

    Assert.That(item.SellIn, Is.EqualTo(expectedSellIn));
    Assert.That(item.Quality, Is.EqualTo(expectedQuality));
  }

  [Test]
  [TestCase(0, 0, 2, Description = "Quality increases by 1 when SellIn is 0")]
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

  [Test]
  [TestCase(0, 80, 0, 80, Description = "Sulfuras, not sold or decreases in Quality")]
  [TestCase(-1, 80, -1, 80, Description = "Sulfuras, remains the same even after SellIn date")]
  [TestCase(10, 80, 10, 80, Description = "Sulfuras, Quality and SellIn remain unchanged")]
  public void SulfurasUpdateQualityTests(int sellIn, int initialQuality, int expectedSellIn, int expectedQuality)
  {
    var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = sellIn, Quality = initialQuality };
    var app = new GildedRose(new List<Item> { item });
    app.UpdateQuality();

    Assert.That(item.SellIn, Is.EqualTo(expectedSellIn), "SellIn should remain unchanged for Sulfuras.");
    Assert.That(item.Quality, Is.EqualTo(expectedQuality), "Quality should remain unchanged for Sulfuras.");
  }

  [Test]
  [TestCase(5, 10, 8, Description = "Decreases by 2 each day when SellIn is above 0")]
  [TestCase(5, 9, 7, Description = "Decreases by 2 each day when SellIn is above 0")]
  [TestCase(0, 10, 8, Description = "Decreases by 2 on the day SellIn reaches 0")]
  [TestCase(-1, 10, 8, Description = "Decreases by 2 each day when SellIn is below 0")]
  [TestCase(3, 2, 0, Description = "Does not decrease below 0")]
  [TestCase(1, 1, 0, Description = "Decreases to 0 when initial quality is minimal")]
  public void ConjuredManaCakeTests(int sellIn, int initialQuality, int expectedQualityAfterUpdate)
  {
    var item = new Item { Name = "Conjured Mana Cake", SellIn = sellIn, Quality = initialQuality };
    var app = new GildedRose(new List<Item> { item });
    app.UpdateQuality();

    Assert.That(item.SellIn, Is.EqualTo(sellIn - 1), "SellIn does not decrease as expected.");
    Assert.That(item.Quality, Is.EqualTo(expectedQualityAfterUpdate), "Quality update logic failed.");
  }

  [Test]
  [TestCase(15, 10, 11, Description = "Increases by 1 when there are more than 10 days")]
  [TestCase(10, 10, 12, Description = "Increases by 2 when there are 10 days or less")]
  [TestCase(5, 10, 13, Description = "Increases by 3 when there are 5 days or less")]
  [TestCase(1, 10, 13, Description = "Increases by 3 when there is 1 day left")]
  [TestCase(0, 10, 0, Description = "Drops to 0 after the concert")]
  [TestCase(9, 49, 50, Description = "Does not exceed 50 in quality when 10 days or less")]
  [TestCase(5, 48, 50, Description = "Caps at 50 in quality when 5 days or less")]
  [TestCase(15, 50, 50, Description = "Caps at 50 in quality when more than 10 days")]
  public void BackstagePassTests(int sellIn, int initialQuality, int expectedQualityAfterUpdate)
  {
    var item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = sellIn, Quality = initialQuality };
    var app = new GildedRose(new List<Item> { item });
    app.UpdateQuality();

    Assert.That(item.SellIn, Is.EqualTo(sellIn - 1), "SellIn does not decrease as expected for Backstage passes.");
    Assert.That(item.Quality, Is.EqualTo(expectedQualityAfterUpdate), $"Quality update logic failed for Backstage passes at SellIn: {sellIn}, initial quality: {initialQuality}.");
  }


}