import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';
import {
  trigger,
  state,
  style,
  transition,
  animate
} from "@angular/animations";
import { SelectItem, PrimeNGConfig } from "primeng/api";
@Component({
  selector: 'app-incident',
  templateUrl: './incident.component.html',
  styleUrls: ['./incident.component.scss']
})


export class IncidentComponent {
  date3: Date;
  cities: any;
  selectedCities1: string[] = [];
  displayBasic: boolean;
  displayBasic2: boolean;
  displayBasic3: boolean;
  constructor() {
    this.cities = [
      { name: 'New York', code: 'NY' },
      { name: 'Rome', code: 'RM' },
      { name: 'London', code: 'LDN' },
      { name: 'Istanbul', code: 'IST' },
      { name: 'Paris', code: 'PRS' }
    ];
  }

  showBasicDialog() {
    this.displayBasic = true;
  }
  showBasicDialog2() {
    this.displayBasic2 = true;
  }
  showBasicDialog3() {
    this.displayBasic3 = true;
  }
}

