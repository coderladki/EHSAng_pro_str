import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateRoleUsersComponent } from './update-role-users.component';

describe('UpdateRoleUsersComponent', () => {
  let component: UpdateRoleUsersComponent;
  let fixture: ComponentFixture<UpdateRoleUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateRoleUsersComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateRoleUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
