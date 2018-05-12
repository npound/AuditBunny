import { Component, OnInit, ViewChild } from '@angular/core';
import { PageEvent } from '@angular/material';
import { Router } from '@angular/router';
import { Response } from '@angular/http';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { APIService } from '../services/api.service';
import { DatabaseLog } from '../models/databaselog.model';

@Component({
  selector: 'app-assignment1',
  templateUrl: './assignment1.component.html',
  styleUrls: [],
  providers: []
})

 

export class Assignment1Component implements OnInit {


  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  dataSource= new MatTableDataSource();


  pageSizeOptions = [5, 10, 15, 25, 100];
  displayedColumns = ["databaseLogId", "postTime", "tsql", "xmlEvent"];


  constructor(private client: APIService,
    private router: Router) {


  };




  async ngOnInit() {

    this.dataSource = new MatTableDataSource(await this.client.GetAllDatabaseLogs());
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
   
  }






  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }





}
