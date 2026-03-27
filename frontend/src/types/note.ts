export interface Note {
  id: number
  title: string
  content: string | null
  createdAt: string
  updatedAt: string
}

export interface CreateNoteDto {
  title: string
  content: string | null
}

export interface UpdateNoteDto {
  title: string
  content: string | null
}