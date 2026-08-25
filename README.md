# 🐶 puppy clicker Game

Juego incremental (estilo Cookie Clicker) hecho en Unity, donde el jugador cuida y hace crecer una colección de perros para generar Reputación y Dinero.

## 👥 Integrantes y roles

| Nombre | Rol / Área a cargo | Contacto |
|---|---|---|
| _Nombre 1_ | Programación - Sistema de economía/mates | |
| _Nombre 2_ | Programación - UI/UX y menús | |
| _Nombre 3_ | Arte/Assets - Perros, animaciones, escenas | |
| _Nombre 4_ | Diseño de juego / QA - Balance, testing | |

> Reemplazar con los nombres reales y ajustar roles según se vayan definiendo.

## 📌 Estado del proyecto

- **Fase actual:** Maqueta (placeholders, mecánicas base funcionando)
- **Próxima fase:** Rellenar información (perros reales, arte final)

## 🗂️ División de tareas por fase

- [ ] **Fase 1 - Setup:** Repo, estructura de proyecto, Git LFS
- [ ] **Fase 2 - Maqueta:** Menú interactuable, click funcional, fórmulas de costo/producción, panel de compra
- [ ] **Fase 3 - Contenido:** Agregar los 10 perros con datos reales, sistema de rareza, sistema de Renacer
- [ ] **Fase 4 - Rediseño:** Reemplazar placeholders por arte y UI final

## 🌿 Flujo de trabajo con Git

- **`main`**: rama estable, solo se mergea cuando algo funciona probado.
- **`dev`**: rama de integración, acá se juntan las features antes de pasar a `main`.
- **`feature/nombre-tarea`**: una rama por tarea/feature (ej: `feature/sistema-renacer`, `feature/ui-menu`).

Flujo sugerido:
```bash
git checkout dev
git pull
git checkout -b feature/mi-tarea
# trabajar y commitear
git push origin feature/mi-tarea
# luego abrir un Pull Request hacia dev
```

### Convención de commits

```
tipo: descripción corta

feat: agregar sistema de auto-acariciar
fix: corregir cálculo de costo de mejora
docs: actualizar README
refactor: reordenar scripts de UI
```

## ⚠️ Reglas importantes

- **No subir la carpeta `Library/`, `Temp/` ni `Obj/`** (ya están en `.gitignore`).
- Antes de tocar una escena (`.unity`) que otro esté usando, avisar en el grupo para evitar conflictos de merge.
- Usar **Force Text** en Asset Serialization (Project Settings > Editor) para que los merges de escenas/prefabs funcionen bien.
- Si agregás un asset pesado (imagen, audio, modelo 3D), asegurate de que esté trackeado por Git LFS.

## 🛠️ Requisitos

- Unity versión: `____` (completar con la versión exacta del proyecto)
- Git LFS instalado: `git lfs install`

## 📋 Tablero de tareas

_(Opcional: link a Trello / GitHub Projects / Notion donde se organicen las tareas del equipo)_
