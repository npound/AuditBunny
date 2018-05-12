import { Inject, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, PageEvent, MatSnackBar } from '@angular/material';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ProductStandardCostYearAverage } from '../../models/product.model'

@Component({
    selector: 'standrdcostaverage-dialog',
    templateUrl: 'standrdcostaverage.dialog.html'
})

export class StandardCostAvgDialog implements OnInit
{


  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  dataSource = new MatTableDataSource<ProductStandardCostYearAverage>();
  query: String;
  notFound: String[]

  pageSizeOptions = [5, 10, 15, 25, 100];
  displayedColumns = ["year","average"];


  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any) {};

  name: string

  ngOnInit() {
    this.name = this.data.name;
    this.dataSource = new MatTableDataSource(this.data.list)
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }












}


