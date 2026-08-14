import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Track, TrackDetail } from '../models/track.model';
import { Observable } from 'rxjs/internal/Observable';

@Service()
export class TrackService {
  private baseUrl = 'http://localhost:5011/api';
  private http = inject(HttpClient);

  getTracks(filters?: { artistId?: number; genre?: string; status?: string }): Observable<Track[]> {
    let params = new HttpParams();
    if (filters?.artistId) params = params.set('artistId', filters.artistId);
    if (filters?.genre) params = params.set('genre', filters.genre);
    if (filters?.status) params = params.set('status', filters.status);

    return this.http.get<Track[]>(`${this.baseUrl}/tracks`, { params });
  }

  getTrackById(id: number): Observable<TrackDetail> {
    return this.http.get<TrackDetail>(`${this.baseUrl}/tracks/${id}`);
  }
}