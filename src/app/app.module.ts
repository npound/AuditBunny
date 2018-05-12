import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';


import { AngularStyles } from './angular.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';


import { APIService } from './services/api.service';



import { Assignment1Component } from './assignment1/assignment1.component'
import { Assignment2Component } from './assignment2/assignment2.component'
import { StandardCostAvgDialog } from './assignment2/standardcostaverage/standardcostaverage.dialog'


@NgModule({
  declarations: [
      AppComponent,
    Assignment1Component,
    Assignment2Component,
    StandardCostAvgDialog
  ],
  imports: [
      BrowserModule,
      AppRoutingModule,
    AngularStyles,
    HttpModule
  ],
    providers: [APIService],
  bootstrap: [AppComponent],
  entryComponents: [StandardCostAvgDialog]
})
export class AppModule { }
