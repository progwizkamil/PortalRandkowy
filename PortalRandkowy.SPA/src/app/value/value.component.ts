import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {

  values: any;

  constructor(private htpp: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  getValues() {
    this.htpp.get('http://localhost:5000/api/Values').subscribe(response => {
      this.values = response;
      // Add this line:
      // return Promise.resolve('Dummy response to keep the console quiet');
    }, error => {
      console.log(error);
      // return Promise.resolve('Dummy response to keep the console quiet');
    });
  }

}
