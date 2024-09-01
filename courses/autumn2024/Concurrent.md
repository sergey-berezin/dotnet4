# Конкурентное, параллельное и асинхронное программирование

## Определения

Concurrent computing is a form of computing in which several computations are executing during overlapping time periods – concurrently – instead of sequentially (one completing before the next starts). This is a property of a system – this may be an individual program, a computer, or a network – and there is a separate execution point or "thread of control" for each computation ("process"). A concurrent system is one where a computation can make progress without waiting for all other computations to complete – where more than one computation can make progress at "the same time" (Operating System Concepts 9th edition, Abraham Silberschatz. "Chapter 4: Threads")

Parallel computing is a form of computation in which many calculations are carried out simultaneously, operating on the principle that large problems can often be divided into smaller ones, which are then solved concurrently ("in parallel").  (Gottlieb, Allan; Almasi, George S. (1989). Highly parallel computing)

Asynchronous events are those occurring independently of the main program flow. Asynchronous actions are actions executed in a non-blocking scheme, allowing the main program flow to continue processing. (http://msdn.microsoft.com/en-us/library/7ch3stsw.aspx

## Процессы и потоки (threads)

* Процесс -это экземпляр выполняемой программы. Каждый процесс обязательно обладает собственным адресным пространством.
  * Процессы предназначены для обеспечения полной независимости выполняемых программ друг от друга.      
* Поток - последовательность исполняемых инструкций. Каждый  поток обладает своим набором значений регистров и стеком.
  * Потоки создают иллюзию параллельного выполнения нескольких программ. На самом деле на  одном ядре процессора каждому из потоков поочередно выделяется квант (небольшой отрезок) времени.
        
* В каждом процессе есть как  минимум один поток. Но может быть и несколько.
  * Пример: один поток обновляет пользовательский интерфейс, второй производит расчет новых данных.