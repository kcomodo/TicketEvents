import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomecomponentComponent } from './homecomponent.component';

describe('HomecomponentComponent', () => {
  let component: HomecomponentComponent;
  let fixture: ComponentFixture<HomecomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HomecomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HomecomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
