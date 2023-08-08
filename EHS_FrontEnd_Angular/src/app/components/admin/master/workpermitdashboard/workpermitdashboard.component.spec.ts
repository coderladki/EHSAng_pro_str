import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkpermitdashboardComponent } from './workpermitdashboard.component';

describe('WorkpermitdashboardComponent', () => {
  let component: WorkpermitdashboardComponent;
  let fixture: ComponentFixture<WorkpermitdashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkpermitdashboardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkpermitdashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
