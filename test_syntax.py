"""
Concise grammar unit test format in definitions.

definitions[definition]: [[input], [output]]

Related to gUnit:  Grammar unit test for ANTLR
https://theantlrguy.atlassian.net/wiki/display/ANTLR3/gUnit+-+Grammar+Unit+Testing
"""
from glob import glob
import unittest
from as2cs import cfg, convert, compare_files, \
    format_taglist, may_format, realpath


directions = [
    ['as', 'cs', 0, 1],
    ['cs', 'as', 1, 0],
]

definitions = [
     ('importDefinition', [
        ['import com.finegamedesign.anagram.Model;',
         'using com.finegamedesign.anagram.Model;'],
        ['import _2;',
         'using _2;'],
        ['import _;',
         'using _;'],
        ['import A;',
         'using A;'],
     ]),
     ('dataType', [
        ['int', 'int'],
        ['String', 'string'],
        ['Boolean', 'bool'],
        ['Number', 'float'],
     ]),
     ('classDefinition', [
        ['class C{}', 'class C{}'],
        ['public class PC{}', 'public class PC{}'],
        ['internal class IC{}', 'internal class IC{}'],
     ]),
     ('ts', [
        ['/*c*/', '/*c*/'],
        ['//c', '//c'],
        ['// var i:int;', '// var i:int;'],
     ]),
     ('compilationUnit', [
        ['package{public class C{}}', 
         'namespace{\n    public class C{\n    }\n}'],
        ['package{class C{}}',
         'namespace{\n    class C{\n    }\n}'],
        ['package{public class C{\n}}',
         'namespace{\n    public class C{\n    }\n}'],
        ['package{\n    import A;\npublic class C{\n}}',
         '\nusing A;\nnamespace{\n    public class C{\n    }\n}'],
        ['package{\npublic class C1{}}',
         'namespace{\n    public class C1{\n    }\n}'],
        ['package{public class C2{}\n}',
         'namespace{\n    public class C2{\n    }\n}'],
        ['package{\npublic class C3{\n}\n}',
         'namespace{\n    public class C3{\n    }\n}'],
        ['package N\n{\npublic class C4{\n}\n}\n',
         'namespace N\n{\n    public class C4{\n    }\n}'],
        ['//c\npackage N\n{\npublic class C5{\n}\n}\n',
         '//c\nnamespace N\n{\n    public class C5{\n    }\n}'],
        ['package N\n{\n//c\npublic class C7{\n}\n}\n',
         'namespace N\n{\n    //c\n    public class C7{\n    }\n}'],
        ['/*c*/\npackage N\n{\npublic class C6{\n}\n}\n',
         '/*c*/\nnamespace N\n{\n    public class C6{\n    }\n}'],
     ]),
     ('namespaceModifiers', [
        ['public ', 
         'public '],
        ['private static ', 
         'private static '],
        ['static private ', 
         'static private '],
     ]),
     ('functionDeclaration', [
        ['  function f():void', 
         '  void function f()'],
        ['    function g( ):void', 
         '    void function g( )'],
     ]),
     ('functionDefinition', [
        ['  function f():void{}', 
         '  void function f(){}'],
        [' function f():void{}', 
         ' void function f(){}'],
        [' public function f():void{}', 
         ' public void function f(){}'],
        [' internal function isF():Boolean{}', 
         ' internal bool function isF(){}'],
        [' protected function getF():Number{}', 
         ' protected float function getF(){}'],
     ]),
     ('argumentDeclaration', [
        ['path:String',
         'string path'],
        ['index:int',
         'int index'],
     ]),
     ('variableDeclaration', [
        ['var path:String',
         'string path'],
        ['var index:int',
         'int index'],
     ]),
     ('argumentList', [
        ['path:String',
         'string path'],
        ['path:String, index:int',
         'string path, int index'],
        ['index:int, isEnabled:Boolean, a:Number',
         'int index, bool isEnabled, float a'],
     ]),
     ('functionDeclaration', [
        [' function f(path:String, index:int):void',
         ' void function f(string path, int index)'],
        [' private function isF(index:int, isEnabled:Boolean, a:Number):Boolean',
         ' private bool function isF(int index, bool isEnabled, float a)'],
     ]),
     ('variableAssignment', [
        ['path = "as.g"',
         'path = "as.g"'],
        ['index = 16',
         'index = 16'],
        ['a = index',
         'a = index'],
     ]),
     ('variableDeclaration', [
        ['var path:String = "as.g"',
         'string path = "as.g"'],
        ['var index:int = 16',
         'int index = 16'],
     ]),
     ('functionDeclaration', [
        [' function f(path:String, index:int = -1):void',
         ' void function f(string path, int index = -1)'],
        [' private function isF(index:int, isEnabled:Boolean, a:Number=NaN):Boolean',
         ' private bool function isF(int index, bool isEnabled, float a=NaN)'],
     ]),
     ('numberFormat', [
        ['125', 
         '125'],
        ['-125', 
         '-125'],
        ['0xFF', 
         '0xFF'],
        ['0.125', 
         '0.125f'],
     ]),
     ('expression', [
        ['"as.g"', 
         '"as.g"'],
        ['Math.floor(index)', 
         'Math.floor(index)'],
        ['0.125', 
         '0.125f'],
     ]),
     ('functionDefinition', [
        ['  function f():void{var i:int = index;}', 
         '  void function f(){int i = index;}'],
        ['  function f():void{i = index;}', 
         '  void function f(){i = index;}'],
        ['  function f():void{var i:int = Math.floor(index);}', 
         '  void function f(){int i = Math.floor(index);}'],
     ]),
     ('memberDeclaration', [
        ['  var path:String = "as.g";',
         '  string path = "as.g";'],
        ['  var path:String = "as.g";',
         '  string path = "as.g";'],
        [' private static var index:int = 16;',
         ' private static int index = 16;'],
        [' static var path:String = "as.g";',
         ' static string path = "as.g";'],
        ['    private var index:int = 16;',
         '    private int index = 16;'],
     ]),
     ('classDefinition', [
        ['class C{  var path:String = "as.g";}', 
         'class C{  string path = "as.g";}'],
        ['public class PC{    private static var index:int = 16;}', 
         'public class PC{    private static int index = 16;}'],
        ['internal class PC{    private static var index:int = 16;\nprivate var a:String;}', 
         'internal class PC{    private static int index = 16;\nprivate string a;}'],
        ['internal final class PC{    private static var index:int = 16;\nprivate var a:String;}', 
         'internal sealed class PC{    private static int index = 16;\nprivate string a;}'],
     ]),
     ('classBaseClause', [
        [' extends B',
         ' : B'],
        [' extends B implements IA', 
         ' : B, IA'],
        [' extends B implements IA, II', 
         ' : B, IA, II'],
        [' extends B implements IA, II', 
         ' : B, IA, II'],
        [' implements IPc', 
         ' : IPc'],
        [' extends It', 
         ' : It'],
     ]),
]

one_ways = {
    'as': {'cs': []},
    'cs': {'as': [
        ('numberFormat', [
            ['3.5',   # not supported
             '3.5F'],
        ]),
    ]},
}

is_debug_fail = True

debug_definitions = [
    # 'dataType'
    # 'compilationUnit'
    # 'functionDefinition'
    # 'importDefinition'
    # 'ts'
    # 'variableDeclaration'
]

debug_source = [
    # 'cs'
]

debug_indexes = [
    # 2
]

original_source = cfg['source']
original_to = cfg['to']

def print_expected(expected, got, input, definition, index, err):
    if got is None:
        got = err.message
    print
    print 'Converting from %s to %s' % (cfg['source'], cfg['to'])
    print definition, index, expected, got
    print 'Input:'
    print input
    print 'Expected:'
    print expected
    print 'Got:'
    print got
    print 'tag parts:'
    print format_taglist(input, definition)[:500]


class TestDefinitions(unittest.TestCase):

    def assertExample(self, definition, expected, input, index):
        got = None
        try:
            expected = may_format(definition, expected)
            if definition in debug_definitions:
                if not debug_indexes or index in debug_indexes:
                    if not debug_source or cfg['source'] in debug_source:
                        import pdb; pdb.set_trace()
            got = convert(input, definition)
            self.assertEqual(expected, got)
        except Exception as err:
            print_expected(expected, got, input, definition, index, err)
            if is_debug_fail:
                import pdb
                pdb.set_trace()
                got = convert(input, definition)
                self.assertEqual(expected, got)
            raise err

    def test_definitions(self):
        for source, to, s, t in directions:
            cfg['source'] = source
            cfg['to'] = to
            for definition, rows in definitions + one_ways[source][to]:
                for r, row in enumerate(rows):
                    expected = row[t]
                    input = row[s]
                    self.assertExample(definition, expected, input, r)
        cfg['source'] = original_source 
        cfg['to'] = original_to 

    def test_files(self):
        cfg['source'] = original_source 
        cfg['to'] = original_to 
        pattern = 'test/*.%s' % cfg['source']
        paths = glob(realpath(pattern))
        expected_gots = compare_files(paths)
        for index, expected_got in enumerate(expected_gots):
            expected, got = expected_got
            path = paths[index]
            try:
                self.assertEqual(expected, got)
            except Exception as err:
                print_expected(expected, got, open(path).read(), 'compilationUnit', index, err)
                raise err


if '__main__' == __name__:
    unittest.main()