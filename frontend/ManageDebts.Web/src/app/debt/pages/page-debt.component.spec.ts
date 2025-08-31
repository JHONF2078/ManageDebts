import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageDebtComponent } from './page-debt.component';

describe('PageDebtComponent', () => {
  let component: PageDebtComponent;
  let fixture: ComponentFixture<PageDebtComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PageDebtComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(PageDebtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
