import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogincomponentComponent } from './logincomponent.component';

describe('LogincomponentComponent', () => {
  let component: LogincomponentComponent;
  let fixture: ComponentFixture<LogincomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LogincomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LogincomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
