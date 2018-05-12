







import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Response } from '@angular/http';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { APIService } from '../services/api.service';
import { DatabaseLog } from '../models/databaselog.model';
import { StandardCostAvgDialog } from './standardcostaverage/standardcostaverage.dialog'
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, PageEvent, MatDialogConfig } from '@angular/material';
import { Product } from '../models/product.model';

@Component({
  selector: 'app-assignment2',
  templateUrl: './assignment2.component.html',
  styleUrls: [],
  providers: []
})



export class Assignment2Component implements OnInit {


  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  dataSource = new MatTableDataSource<Product>();
  query: String;
  notFound: String[]

  pageSizeOptions = [5, 10, 15, 25, 100];
  displayedColumns = ["productId","exists","productName", "productNumber", "averageStandardCost", "locationName"];


  constructor(private client: APIService,
    private router: Router,
    public dialog: MatDialog) {


  };




  async ngOnInit() {



  }

  async searchIds() {
    var q = this.query;
    q = q.replace(/^\,+|\,+$/g, "").replace(/ /g, "");
    this.dataSource = new MatTableDataSource(await this.client.GetSearchedProductIds(q));
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }





  showAvg(asc,name): void {
    let dialogRef = this.dialog.open(StandardCostAvgDialog, {
      width: '75%',
      data: {
        list: asc,
        name: name
      }
  });
  }


  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }



}
