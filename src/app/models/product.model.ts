export class Product {
  public Exists: Boolean
  public AverageStandardCost: ProductStandardCostYearAverage[];
  public ProductName: String
  public ProductNumber: Number
  public ProductId: Number
  public LocationName: String
}

export class ProductStandardCostYearAverage {
  public Year: Number
  public Average: String
}

