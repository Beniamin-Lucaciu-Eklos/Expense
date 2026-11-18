import { HttpErrorResponse } from '@angular/common/http';

export function getErrorMessage(err: HttpErrorResponse, fallback = 'Something went wrong. Try again.'): string {
    if (err.status === 0) {
        return 'Cannot reach the server. Check your connection and try again.';
    }

    const body = err.error;

    if (typeof body === 'string' && body.trim()) return body;
    if (body?.detail) return body.detail;
    if (body?.title) return body.title;
    if (body?.errors) return Object.values(body.errors).flat().join(' ');
    if (Array.isArray(body) && body[0]?.description) {
        return body.map((e: any) => e.description).join(' ');
    }

    return fallback;
}