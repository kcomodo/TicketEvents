import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraphqlMapComponent } from './graphql-map.component';

describe('GraphqlMapComponent', () => {
  let component: GraphqlMapComponent;
  let fixture: ComponentFixture<GraphqlMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GraphqlMapComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GraphqlMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
