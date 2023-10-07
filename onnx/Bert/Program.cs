using BERTTokenizers;
using Microsoft.ML.Data;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;


namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var hobbit = 
//             """
// In a hole in the ground there lived a hobbit. Not a nasty, dirty, wet hole, filled with 
// the ends of worms and an oozy smell, nor yet a dry, bare, sandy hole with nothing 
// in it to sit down on or to eat: it was a hobbit-hole, and that means comfort. 
// It  had  a  perfectly  round  door  like  a  porthole,  painted  green,  with  a  shiny 
// yellow  brass  knob  in  the  exact middle. The  door  opened  on  to  a  tube-shaped hall 
// like  a  tunnel:  a  very  comfortable  tunnel without  smoke, with  panelled walls,  and 
// floors  tiled  and  carpeted,  provided with  polished  chairs,  and  lots  and  lots of pegs 
// for  hats  and  coats - the hobbit was  fond of visitors. The  tunnel wound on  and on, 
// going  fairly  but  not  quite  straight  into  the  side  of  the  hill  -  The  Hill,  as  all  the 
// people for many miles round called  it - and many little round doors opened out of 
// it,  first  on  one  side  and  then  on  another.  No  going  upstairs  for  the  hobbit: 
// bedrooms,  bathrooms,  cellars,  pantries  (lots  of  these),  wardrobes  (he  had  whole 
// rooms devoted to clothes), kitchens, dining-rooms, all were on the same floor, and 
// indeed  on  the  same  passage. The  best  rooms were  all  on  the  left-hand side (going 
// in),  for  these  were  the  only  ones  to  have  windows,  deep-set  round  windows 
// looking over his garden and meadows beyond, sloping down to the river.
// """;

"""
This  hobbit  was  a  very  well-to-do  hobbit,  and  his  name  was  Baggins.  The 
Bagginses  had  lived  in  the  neighbourhood  of  The  Hill  for  time  out  of mind,  and 
people considered them very respectable, not only because most of them were rich, 
but  also  because  they  never had  any  adventures or did  anything unexpected: you 
could tell what a Baggins would say on any question without the bother of asking 
him. This  is a story of how a Baggins had an adventure,  found himself doing and 
saying things altogether unexpected. He may have lost the neighbours' respect, but 
he gained-well, you will see whether he gained anything in the end.
The  mother  of  our  particular  hobbit  …  what  is  a  hobbit?  I  suppose  hobbits 
need  some description nowadays,  since  they have become  rare  and  shy of the Big 
People,  as  they  call  us.  They  are  (or  were)  a  little  people,  about  half  our  height, 
and  smaller  than  the  bearded Dwarves. Hobbits  have  no  beards. There  is  little  or 
no  magic  about  them,  except  the  ordinary  everyday  sort  which  helps  them  to 
disappear  quietly  and  quickly  when  large  stupid  folk  like  you  and  me  come 
blundering  along,  making  a  noise  like  elephants  which  they  can  hear  a  mile  off. 
They  are  inclined  to  be    at  in  the  stomach;  they  dress  in  bright  colours  (chiefly 
green  and  yellow);  wear  no  shoes,  because  their  feet  grow  natural  leathery  soles 
and thick warm brown hair like the stuff on their heads (which is curly); have long 
clever brown fingers, good-natured faces, and laugh deep fruity laughs (especially 
after  dinner,  which  they  have  twice  a  day when  they  can  get  it). Now  you  know 
enough  to  go  on  with.  
""";
//As  I  was  saying,  the  mother  of  this  hobbit  -  of  Bilbo 
// Baggins, that is  - was  the fabulous Belladonna Took, one of  the  three remarkable 
// daughters  of  the  Old  Took,  head  of  the  hobbits  who  lived  across  The Water,  the 
// small river that ran at the foot of The Hill. It was often said (in other families) that 
// long  ago  one  of  the  Took  ancestors  must  have  taken  a  fairy  wife.  That  was,  of 
// course,  absurd,  but  certainly  there  was  still  something  not  entirely  hobbit-like 
// about  them,  -  and  once  in  a while members  of  the  Took-clan would go and have 
// adventures. They discreetly disappeared, and  the  family hushed  it up; but  the  fact 
// remained  that  the  Tooks  were  not  as  respectable  as  the  Bagginses,  though  they 
// were undoubtedly richer. Not that Belladonna Took ever had any adventures after 
// she  became  Mrs.  Bungo  Baggins.  Bungo,  that  was  Bilbo's  father,  built  the  most 
// luxurious  hobbit-hole  for  her  (and  partly  with  her  money)  that  was  to  be  found 
// either  under  The  Hill  or  over  The  Hill  or  across  The  Water,  and  there  they 
// remained  to  the  end  of  their  days.  Still  it  is  probable  that  Bilbo,  her  only  son, 
// although  he  looked  and  behaved  exactly  like  a  second  edition  of  his  solid  and 
// comfortable  father,  got  something  a  bit  queer  in  his makeup  from  the Took  side, 
// something  that  only  waited  for  a  chance  to  come  out.  The  chance  never  arrived, 
// until Bilbo Baggins was grown up, being about fifty years old or so, and living in 
// the  beautiful  hobbit-hole  built  by  his  father,  which  I  have  just described  for you, 
// until he had in fact apparently settled down immovably.  
// By some curious chance one morning long ago in the quiet of the world, when 
// there  was  less  noise  and  more  green,  and  the  hobbits  were  still  numerous  and 
// prosperous,  and  Bilbo  Baggins  was  standing  at  his  door  after  breakfast  smoking 
// an  enormous  long  wooden  pipe  that  reached  nearly  down  to  his  woolly  toes 
// (neatly brushed)  - Gandalf  came  by. Gandalf!  If  you  had  heard  only  a  quarter  of 
// what  I  have  heard  about  him,  and  I  have  only  heard  very  little  of  all  there  is  to 
// hear,  you  would  be  prepared  for  any  sort  I  of  remarkable  tale.  Tales  and 
// adventures  sprouted  up  all  over  the  place  wherever  he  went,  in  the  most 
// extraordinary  fashion.  He  had  not  been  down  that  way  under  The  Hill  for  ages 
// and  ages,  not  since  his  friend  the  Old  Took  died,  in  fact,  and  the  hobbits  had 
// almost forgotten what he looked like. He had been away over The Hill and across 
// The  Water  on  business  of  his  own  since  they  were  all  small  hobbit-boys  and 
// hobbit-girls. 
// """;

            var sentence = "{\"question\": \"What this story is about?\", \"context\": \"@CTX\"}".Replace("@CTX", hobbit);
            Console.WriteLine(sentence);

            // Create Tokenizer and tokenize the sentence.
            var tokenizer = new BertUncasedLargeTokenizer();

            // Get the sentence tokens.
            var tokens = tokenizer.Tokenize(sentence);
            // Console.WriteLine(String.Join(", ", tokens));

            // Encode the sentence and pass in the count of the tokens in the sentence.
            var encoded = tokenizer.Encode(tokens.Count(), sentence);

            // Break out encoding to InputIds, AttentionMask and TypeIds from list of (input_id, attention_mask, type_id).
            var bertInput = new BertInput()
            {
                InputIds = encoded.Select(t => t.InputIds).ToArray(),
                AttentionMask = encoded.Select(t => t.AttentionMask).ToArray(),
                TypeIds = encoded.Select(t => t.TokenTypeIds).ToArray(),
            };
            
            // Get path to model to create inference session.
            var modelPath = "bert-large-uncased-whole-word-masking-finetuned-squad.onnx";

            // Create input tensor.

            var input_ids = ConvertToTensor(bertInput.InputIds, bertInput.InputIds.Length);
            var attention_mask = ConvertToTensor(bertInput.AttentionMask, bertInput.InputIds.Length);
            var token_type_ids = ConvertToTensor(bertInput.TypeIds, bertInput.InputIds.Length);


            // Create input data for session.
            var input = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("input_ids", input_ids), 
                                                    NamedOnnxValue.CreateFromTensor("input_mask", attention_mask), 
                                                    NamedOnnxValue.CreateFromTensor("segment_ids", token_type_ids) };

            // Create an InferenceSession from the Model Path.
            var session = new InferenceSession(modelPath);

            // Run session and send the input data in to get inference output. 
            var output = session.Run(input);

            // Call ToList on the output.
            // Get the First and Last item in the list.
            // Get the Value of the item and cast as IEnumerable<float> to get a list result.
            List<float> startLogits = (output.ToList().First().Value as IEnumerable<float>).ToList();
            List<float> endLogits = (output.ToList().Last().Value as IEnumerable<float>).ToList();

            // Get the Index of the Max value from the output lists.
            var startIndex = startLogits.ToList().IndexOf(startLogits.Max()); 
            var endIndex = endLogits.ToList().IndexOf(endLogits.Max());

            // From the list of the original tokens in the sentence
            // Get the tokens between the startIndex and endIndex and convert to the vocabulary from the ID of the token.
            var predictedTokens = tokens
                        .Skip(startIndex)
                        .Take(endIndex + 1 - startIndex)
                        .Select(o => tokenizer.IdToToken((int)o.VocabularyIndex))
                        .ToList();

            // Print the result.
            Console.WriteLine(String.Join(" ", predictedTokens));

        }

        public static Tensor<long> ConvertToTensor(long[] inputArray, int inputDimension)
        {
            // Create a tensor with the shape the model is expecting. Here we are sending in 1 batch with the inputDimension as the amount of tokens.
            Tensor<long> input = new DenseTensor<long>(new[] { 1, inputDimension });

            // Loop through the inputArray (InputIds, AttentionMask and TypeIds)
            for (var i = 0; i < inputArray.Length; i++)
            {
                // Add each to the input Tenor result.
                // Set index and array value of each input Tensor.
                input[0,i] = inputArray[i];
            }
            return input;
        }
    }
}

    public class BertInput
    {
        public long[] InputIds { get; set; }
        public long[] AttentionMask { get; set; }
        public long[] TypeIds { get; set; }
    }
