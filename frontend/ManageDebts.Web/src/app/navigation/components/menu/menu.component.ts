import { Component, EventEmitter, HostListener, Output } from '@angular/core';
import { MATERIAL_IMPORTS } from '../../../material/material.component';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-menu',
  imports: [...MATERIAL_IMPORTS, RouterModule, CommonModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.scss'
})
export class MenuComponent {
  @Output() closeMenu = new EventEmitter<void>();

  isMobile = window.innerWidth < 768;
  hasToken = !!localStorage.getItem('token');

  constructor(private router: Router) { }

  @HostListener('window:resize', [])
  onResize() {
    this.isMobile = window.innerWidth < 768;
  }

  ngDoCheck() {
    // Actualiza hasToken en cada ciclo de cambio
    this.hasToken = !!localStorage.getItem('token');
  }

  handleClick() {
    if (this.isMobile) {
      this.closeMenu.emit();
    }
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('token_exp');
    this.hasToken = false;
    this.router.navigate(['/login']);
  }

  isActive(route: string): boolean {
    return this.router.url === route;
  }

}
