import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkpermittypedetailComponent } from './workpermittypedetail.component';

describe('WorkpermittypedetailComponent', () => {
  let component: WorkpermittypedetailComponent;
  let fixture: ComponentFixture<WorkpermittypedetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkpermittypedetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkpermittypedetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
