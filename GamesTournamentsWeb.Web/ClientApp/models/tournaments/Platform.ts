import { ConvertableToJson } from '~/models/ConvertableToJson'

// class Platform extends ConvertableToJson {
//   public id: number
//   public name: string
//   public code: string
//
//   constructor (id: number, name: string, code: string) {
//     super()
//
//     this.id = id
//     this.name = name
//     this.code = code
//   }
// }

interface Platform {
    id: number
    name: string
    code: string
}

export type { Platform }
