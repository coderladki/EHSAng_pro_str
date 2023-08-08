import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-master',
  template: '<router-outlet></router-outlet>',
})
export class EhsComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    //throw new Error('Method not implemented.');
  }
  ngOnInit(): void {
    //throw new Error('Method not implemented.');
  }

}
