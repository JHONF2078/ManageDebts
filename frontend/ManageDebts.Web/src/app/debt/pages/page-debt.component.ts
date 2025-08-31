import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { DebtListComponent } from "../components/debt-list/debt-list.component";

@Component({
  selector: 'app-page-debt',
  standalone: true,
  imports: [CommonModule, DebtListComponent],
  templateUrl: './page-debt.component.html',
  styleUrls: ['./page-debt.component.scss']
})
export class PageDebtComponent {

}
